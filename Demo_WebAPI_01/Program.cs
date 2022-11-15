using Demo_WebAPI_01.BLL.Interfaces;
using Demo_WebAPI_01.BLL.Services;
using Demo_WebAPI_01.Services;
using Demo_WebAPI_01.SwaggerFilter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IPersonnageService, PersonnageService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddTransient<JwtAuthentificationService>();

builder.Services.AddSingleton<SingletonService>();
builder.Services.AddScoped<ScopedService>();
builder.Services.AddTransient<TransientService>();


// Add Controllers
builder.Services.AddControllers(
    config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
    }
).AddXmlDataContractSerializerFormatters()
 .AddNewtonsoftJson();
// Exemple de Formatters "externe" pour gérer les CSV
// https://github.com/damienbod/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core.Formatter.Csv


// Pour ajouter un logging avec NLog
// https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-6

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{

    // Ajout de la sécurité dans le swagger
    // https://github.com/domaindrivendev/Swashbuckle.AspNetCore#add-security-definitions-and-requirements-for-bearer-auth
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // Ajout du "lock" sur toute les routes
    //  - Solution Simple : Ajout sur toutes les routes
    /*
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
            }
        });
    */

    //  - Solution Complexe : Filter custom pour l'ajouter sur les routes "Authorize"
    option.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Add Cors config
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Exemple", config =>
    {
        // Tout autoriser
        //config.AllowAnyOrigin();
        //config.AllowAnyHeader();
        //config.AllowAnyMethod();

        // Config custom
        config.WithOrigins("http://127.0.0.1:5500");
        config.WithHeaders(
            "Access-Control-Allow-Origin"
        );
        config.WithMethods("GET", "POST", "PUT", "DELETE");  // Sans effet pour 'GET', 'POST', 'HEAD'
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Activation du Cors (Ne pas oublier ;) )
app.UseCors("Exemple");

app.MapControllers();

app.Run();

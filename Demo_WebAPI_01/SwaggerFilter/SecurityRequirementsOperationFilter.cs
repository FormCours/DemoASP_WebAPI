using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Demo_WebAPI_01.SwaggerFilter
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        // Filter custom basé sur https://github.com/domaindrivendev/Swashbuckle.AspNetCore#add-security-definitions-and-requirements

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Verification si la méthode possede un attribut "Authorize"
            var authAttributMethod = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct();

            bool requiredAuthorize = authAttributMethod.Any();

            // Si la méthode n'a pas l'attribut "Authorize". On verifie les attributs de la classe
            if(!requiredAuthorize)
            {
                var authAttributClass = context.MethodInfo.ReflectedType
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Select(attr => attr.Policy)
                    .Distinct();

                // Si la classe est "Authorize", on verifie si la méthode n'a pas "AllowAnonymous" 
                if (authAttributClass.Any())
                {
                    var anonymousAttributMethod = context.MethodInfo
                        .GetCustomAttributes(true)
                        .OfType<AllowAnonymousAttribute>()
                        .Distinct();

                    requiredAuthorize = !anonymousAttributMethod.Any();
                }
            }
            
            // Traitement quand la méthode necessite une authentification
            if (requiredAuthorize)
            {
                // Ajout des "ResponseType" 401 et 403
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                // Ajout de la gestion du jwt sur la route
                var bearerScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ bearerScheme ] = new string[] {}
                    }
                };
            }
        }
    }
}
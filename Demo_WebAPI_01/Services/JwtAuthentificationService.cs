using Demo_WebAPI_01.BLL.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo_WebAPI_01.Services
{
    public class JwtAuthentificationService
    {

        IConfiguration _Configuration;

        public JwtAuthentificationService(IConfiguration config)
        {
            _Configuration = config;
        }

        public string CreateToken(Member member)
        {
            SymmetricSecurityKey sigingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT:Secret"]));

            SigningCredentials credentials = new SigningCredentials(sigingKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                claims: new Claim[]
                {
                  new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),  
                  new Claim(ClaimTypes.Role, member.Role)
                },
                issuer: _Configuration["JWT:Issuer"],
                audience: _Configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}

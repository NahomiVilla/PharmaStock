using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PharmaStock.Models;

namespace PharmaStock.Services{
    public class AuthService{
        private readonly IConfiguration _config;
        public AuthService(IConfiguration config){
            _config = config;
        }

        public string GenerateJWTToken(Users user){
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ArgumentNullException("Jwt:Key", "JWT key cannot be null or empty.");
            }
            var keyBytes=Convert.FromBase64String(jwtKey);
            var key = Encoding.UTF8.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Rol)
                }),
                Expires = System.DateTime.UtcNow.AddHours(1),
                Issuer=_config["Jwt:Issuer"],
                Audience=_config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business
{
    public class JWTService
    {
        private readonly IConfiguration _config;
        public JWTService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string userId, string username, List<string> roles)
        {
            var settings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings["key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
         {
             new Claim(JwtRegisteredClaimNames.Sub, userId),
             new Claim(JwtRegisteredClaimNames.Email, username),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
         };

            roles.ForEach(rol => claims.Add(new Claim(ClaimTypes.Role, rol)));

            var token = new JwtSecurityToken(
                issuer: settings["Issuer"],
                audience: settings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(settings["ExpiresInMinutes"])),
                signingCredentials: creds

             );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}

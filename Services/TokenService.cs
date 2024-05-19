using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blog.Models;
using blogapi.Controller;
using blogapi.Extensios;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.IdentityModel.Tokens;

namespace blogapi.Service
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
          
            var key =  Encoding.ASCII.GetBytes(Configuration.JwtToken);
            //var claim = user.GetClaims();
            var tokenDescriptor = new SecurityTokenDescriptor
       
            {
                Expires = DateTime.UtcNow.AddHours(3),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Fruta","banana"),
                    new Claim(ClaimTypes.Role, "admin"),//User.IsInrole
                    new Claim(ClaimTypes.Role, "user"),
                    new Claim(ClaimTypes.Name, "Bruno Seraguza") //User.Identity.Name

                }),//new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor); 

            return tokenHandler.WriteToken(token);

        }
    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FundooNotes.JWTAuthentication
{
    public class UserAuthenticationJWT
    {
        private IConfiguration config;

        public UserAuthenticationJWT(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            IEnumerable<Claim> Claims = new Claim[] { new Claim("FirstName", userInfo.FirstName), 
                new Claim("LastName", userInfo.LastName),
                new Claim("Email", userInfo.Email) };

            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
              claims: Claims,
              expires: DateTime.Now.AddSeconds(300),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

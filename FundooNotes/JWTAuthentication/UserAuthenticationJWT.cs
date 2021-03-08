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
        List<JwtSecurityToken> TokenList;
        private IConfiguration config;

        public UserAuthenticationJWT(IConfiguration config)
        {
            this.config = config;
            TokenList = new List<JwtSecurityToken>();
        }

        public string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IEnumerable<Claim> Claims = new Claim[] { new Claim("email", userInfo.Email) };

            var token = new JwtSecurityToken(config["Jwt:Issuer"], userInfo.Email,
              claims: Claims,
              expires: DateTime.Now.AddMinutes(1),
              signingCredentials: credentials);
            TokenList.Add(token);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

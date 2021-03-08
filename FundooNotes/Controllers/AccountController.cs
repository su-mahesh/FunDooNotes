using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BusinessLayer.Interfaces;
using CommonLayer.Model;
using FundooNotes.JWTAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        IUserRegistrationBL userRegistrationsBL;
        UserAuthenticationJWT userAuthentication;
        private IConfiguration config;

        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserRegistrationBL userRegistrationsBL, ILogger<AccountController> logger, IConfiguration config)
        {
            this.userRegistrationsBL = userRegistrationsBL;
            _logger = logger;
            this.config = config;
            userAuthentication = new UserAuthenticationJWT(this.config);
        }

        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(UserModel user)
        {
            if (user == null)
            {
                return BadRequest("user is null.");
            }
            try
            {
                bool result = userRegistrationsBL.RegisterUser(user);
                if (result)
                {
                    return Ok(new { success = true, Message = "User Registration Successful" });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "User Registration Unsuccessful" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }                     
        }

        [HttpPost("Login")]
        public IActionResult AuthenticateUser(UserModel user)
        {
            if (user == null)
            {
                return BadRequest("user is null.");
            }
            try
            {
                bool result = userRegistrationsBL.AuthenticateUser(user);
                if (result)
                {
                    var tokenString = userAuthentication.GenerateJSONWebToken(user);
                    return Ok(new { success = true, Message = "User Login Successful", token = tokenString });
                }
                    return BadRequest(new { success = false, Message = "User Login Unsuccessful" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        [HttpGet]
   //     [Authorize]
        public IActionResult GetActiveUser()
        {
            var currentUser = HttpContext.User;

            var accessToken = Request.Headers[HeaderNames.Authorization];
            string[] tokenPart = accessToken.ToString().Split(".");
            if (currentUser.HasClaim(c => c.Type == "email"))
            {
                DateTime date = DateTime.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "DateOfJoing").Value);

                
                return Ok(new { success = true, Message = "User is active", user = currentUser.Claims.FirstOrDefault(c => c.Type == "email").Value });
            }

            return null;
        }
       /* private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IEnumerable<Claim> Claims = new Claim[] { new Claim("email", userInfo.Email) };

            var token = new JwtSecurityToken(config["Jwt:Issuer"], userInfo.Email,
              claims: Claims,
              expires: DateTime.Now.AddMinutes(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/
    }
}

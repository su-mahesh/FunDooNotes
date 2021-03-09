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
using Newtonsoft.Json.Linq;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        IUserAccountBL userAccountBL;
        UserAuthenticationJWT userAuthentication;
        private IConfiguration config;

        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserAccountBL userRegistrationsBL, ILogger<AccountController> logger, IConfiguration config)
        {
            this.userAccountBL = userRegistrationsBL;
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
                UserModel result = userAccountBL.RegisterUser(user);               
                if (result != null)
                {
                    var NewUser = new
                    {
                        result.UserID,
                        result.FirstName,
                        result.LastName,
                        result.Email
                    };

                    return Ok(new { success = true, Message = "User Registration Successful", user = NewUser });
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
                UserModel result = userAccountBL.AuthenticateUser(user);
                if (result != null)
                {
                    var tokenString = userAuthentication.GenerateJSONWebToken(result);
                    var LoginUser = new
                    {
                        result.UserID,
                        result.FirstName,
                        result.LastName,
                        result.Email
                    };
                    return Ok(new { success = true, Message = "User Login Successful", user = LoginUser,
                        token = tokenString });
                }
                    return BadRequest(new { success = false, Message = "User Login Unsuccessful" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAuthorizedUser()
        {          
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;
                    UserModel result = userAccountBL.GetAuthorizedUser(Email);
                    var LoginUser = new
                    {
                        result.UserID,
                        result.FirstName,
                        result.LastName,
                        result.Email
                    };
                    return Ok(new { success = true, Message = "User is active", user = LoginUser });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }              
    }
}

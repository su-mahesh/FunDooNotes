using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BusinessLayer.Interfaces;
using CommonLayer.EmailMessageModel;
using CommonLayer.Model;
using CommonLayer.RequestModel;
using FundooNotes.JWTAuthentication;
using FundooNotes.MSMQ;
using FundooNotes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        IUserAccountBL userAccountBL;
        EmailService emailSevice;
        UserAuthenticationJWT userAuthentication;
        MSMQService msmq;
        private IConfiguration config;

        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserAccountBL userRegistrationsBL, ILogger<AccountController> logger, IConfiguration config)
        {
            emailSevice = new EmailService(config);
            this.userAccountBL = userRegistrationsBL;
            _logger = logger;
            this.config = config;
            userAuthentication = new UserAuthenticationJWT(this.config);
            msmq = new MSMQService(config);
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
                    var tokenString = userAuthentication.GenerateSessionJWT(result);
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
        [Authorize]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;
                    resetPasswordModel.Email = Email;
                    bool result = userAccountBL.ResetPassword(resetPasswordModel);
                    if (result)
                    {
                        return Ok(new { success = true, Message = "password changed successfully" });
                    }
                }
                return BadRequest(new { success = false, Message = "password change unsuccessfull" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        [HttpGet("ForgetPassword")]
        public IActionResult ResetForgottenPassword(ForgetPasswordModel forgetPasswordModel)
        {
            try
            {
                UserModel result = userAccountBL.GetAuthorizedUser(forgetPasswordModel.Email);
               
                if (result != null)
                {
                    var JwtToken = userAuthentication.GeneratePasswordResetJWT(result);
                    ResetLinkEmailModel resetLink = new ResetLinkEmailModel
                    {
                        Email = result.Email,
                        JwtToken = JwtToken
                    };
                    msmq.SendPasswordResetMessage(resetLink);
                    return Ok(new { success = true, Message = "password reset link has been sent to your email id", email = forgetPasswordModel.Email });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "email id don't exist" });
                }                            
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
    }
}

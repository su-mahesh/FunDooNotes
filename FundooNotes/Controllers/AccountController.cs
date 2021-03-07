using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        IUserRegistrationBL userRegistrationsBL;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserRegistrationBL userRegistrationsBL, ILogger<AccountController> logger)
        {
            this.userRegistrationsBL = userRegistrationsBL;
            _logger = logger;
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
        public IActionResult LoggingUser(UserModel user)
        {
            if (user == null)
            {
                return BadRequest("user is null.");
            }
            try
            {
                bool result = userRegistrationsBL.LoggingUser(user);
                if (result)
                {
                    return Ok(new { success = true, Message = "User Login Successful" });
                }
                    return BadRequest(new { success = false, Message = "User Login Unsuccessful" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {           
            try
            {
                IEnumerable<UserModel> result = userRegistrationsBL.GetAllUsers();
                if (result == null)
                {
                    return BadRequest(new { Message = "unsuccessful" });
                }
                return Ok(new { Message = "Successful", data = result });
            }          
            catch (Exception exception)
            {
                return this.BadRequest(new { success = false, exception.Message});
            }  
        }
    }
}

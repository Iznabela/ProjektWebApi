using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjektWebApi.Data;
using ProjektWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;

namespace ProjektWebApi.Controllers
{
    [Route("api/v{version:apiVersion}/register-user")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]

    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<MyUser> _userManager;

        public RegistrationController(UserManager<MyUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(string firstName, string lastName, string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(userName))
            {
                return BadRequest();
            }

            try
            {
                MyUser newUser = new MyUser();
                newUser.FirstName = firstName;
                newUser.LastName = lastName;
                newUser.UserName = userName;
                newUser.PasswordHash = _userManager.PasswordHasher.HashPassword(newUser, password);

                await _userManager.CreateAsync(newUser);

                return Ok();

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}

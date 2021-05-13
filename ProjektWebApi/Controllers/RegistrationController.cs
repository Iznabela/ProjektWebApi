using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektWebApi.Models;
using System;
using System.Threading.Tasks;

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
                MyUser newUser = new MyUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = userName,
                };

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

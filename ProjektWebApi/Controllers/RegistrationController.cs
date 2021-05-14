using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektWebApi.Data;
using ProjektWebApi.Models;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(
                Summary = "Register a user",
                Description = "Fill in credentials to create an account"
                )]
        [SwaggerResponse(201, "User was registered successfully", typeof(MyUser))]
        [SwaggerResponse(400, "Something went wrong with the request")]
        public async Task<IActionResult> RegisterUser(
            [FromQuery, SwaggerParameter("First Name", Required = true)] string firstName,
            [FromQuery, SwaggerParameter("Last Name", Required = true)] string lastName,
            [FromQuery, SwaggerParameter("Username", Required = true)] string userName,
            [FromQuery, SwaggerParameter("Password - you can use letters, numbers and periods", Required = true)] string password)
        {
            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName) || String.IsNullOrWhiteSpace(password))
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

                return CreatedAtAction(nameof(RegisterUser), new { id = newUser.Id }, newUser);

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}

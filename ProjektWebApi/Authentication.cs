using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjektWebApi.Data;
using ProjektWebApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ProjektWebApi
{
    public class Authentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private UserManager<MyUser> _userManager;
        private ApplicationDbContext _context;

        public Authentication(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserManager<MyUser> userManager,
            ApplicationDbContext context)

            : base(options, logger, encoder, clock)
        {
            _userManager = userManager;
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No Authorization Header");

            MyUser user;
            string password;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                password = credentials[1];

                user = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            bool matchingCredentials = await _userManager.CheckPasswordAsync(user, password);

            if (user == null || password == null || !matchingCredentials)
                return AuthenticateResult.Fail("Invalid Username or Password");

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        [SwaggerResponse(401, "Incorrect username or password")]
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;

            Response.Headers["WWW-Authenticate"] = "Basic";

            return Task.CompletedTask;
        }
    }

 }

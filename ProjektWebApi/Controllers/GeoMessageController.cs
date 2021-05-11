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

namespace ProjektWebApi.Controllers
{
    namespace V1
    {
        [Route("api/v{version:apiVersion}/[controller]")]
        [ApiController]
        [ApiVersion("1.0")]
        public class GeoMessageController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<MyUser> _userManager;

            public GeoMessageController(ApplicationDbContext context, UserManager<MyUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public class GeoMassageDTO
            {
                public int Id { get; set; }
                public string Message { get; set; }
                public double Longitude { get; set; }
                public double Latitude { get; set; }
            }

            [Route("GetGeoMessage")]
            [HttpGet]
            public async Task<ActionResult<GeoMessage>> GetGeoMessage(int? id)
            {
                try
                {
                    var geoMessage = await _context.GeoMessages.Include(g => g.Message).FirstOrDefaultAsync(g => g.Id == id);

                    if (geoMessage != null)
                    {
                        return Ok(geoMessage);
                    }
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }

                return NotFound();
            }

            [HttpGet]
            public async Task<ActionResult<ICollection<GeoMessage>>> GetGeoMessages()
            {
                try
                {
                    var geoMessages = await _context.GeoMessages.Include(g => g.Message).ToListAsync();
                    if (geoMessages != null)
                    {
                        return Ok(geoMessages);
                    }
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }

                return NotFound();
            }

            [Authorize]
            [Route("CreateGeoMessage")]
            [HttpPost]
            public async Task<IActionResult> CreateGeoMessage(GeoMassageDTO newGeoMessage)
            {
                if (String.IsNullOrWhiteSpace(newGeoMessage.Message))
                {
                    return BadRequest();
                }

                try
                {
                    MyUser user;
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                    var username = credentials[0];

                    try
                    {
                        user = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();

                        var geoMessage = new GeoMessage();
                        var message = new Message { Title = "Unknown Title", Author = user.FirstName + " " + user.LastName, Body = newGeoMessage.Message };
                        geoMessage.Message = message;
                        geoMessage.Latitude = newGeoMessage.Latitude;
                        geoMessage.Longitude = newGeoMessage.Longitude;
                        await _context.GeoMessages.AddAsync(geoMessage);
                        await _context.SaveChangesAsync();
                        return CreatedAtAction(nameof(GetGeoMessage), new { id = geoMessage.Id }, geoMessage);
                    }
                    catch
                    {
                        return BadRequest();
                    }
                }
                catch
                {
                    return BadRequest();
                }
            }

            [Route("RegisterUser")]
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

    namespace V2
    {
        [Route("api/v{version:apiVersion}/[controller]")]
        [ApiController]
        [ApiVersion("2.0")]
        public class GeoMessageController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<MyUser> _userManager;

            public GeoMessageController(ApplicationDbContext context, UserManager<MyUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            [Route("GetGeoMessage")]
            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<GeoMessage>> GetGeoMessage(int? id)
            {
                try
                {
                    var geoMessage = await _context.GeoMessages.Include(g => g.Message).FirstOrDefaultAsync(g => g.Id == id);
                    if (geoMessage != null)
                    {
                        return Ok(geoMessage);
                    }
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }

                return NotFound();
            }

            [HttpGet]
            public async Task<ActionResult<ICollection<GeoMessage>>> GetGeoMessages(double? minLon, double? minLat, double? maxLon, double? maxLat)
            {
                try
                {
                    if (minLon == null || minLat == null || maxLon == null || maxLat == null)
                    {
                        var geoMessages = await _context.GeoMessages.Include(g => g.Message).ToListAsync();
                        if (geoMessages != null)
                        {
                            return Ok(geoMessages);
                        }
                    }
                    else
                    {
                        var geoMessages = await _context.GeoMessages.Where(g =>
                            g.Longitude >= minLon &&
                            g.Latitude >= minLat &&
                            g.Longitude <= maxLon &&
                            g.Latitude <= maxLat).Include(g => g.Message).ToListAsync();
                        if (geoMessages != null)
                        {
                            return Ok(geoMessages);
                        }
                    }
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }

                return NotFound();
            }

            [Authorize]
            [Route("CreateGeoMessage")]
            [HttpPost]
            public async Task<IActionResult> CreateGeoMessage(GeoMessage newGeoMessage)
            {
                if (String.IsNullOrWhiteSpace(newGeoMessage.Message.Title))
                {
                    return BadRequest();
                }

                try
                {
                    var geoMessage = new GeoMessage();
                    geoMessage.Message = newGeoMessage.Message;
                    geoMessage.Latitude = newGeoMessage.Latitude;
                    geoMessage.Longitude = newGeoMessage.Longitude;
                    await _context.GeoMessages.AddAsync(newGeoMessage);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetGeoMessage), new { id = geoMessage.Id }, geoMessage);
                }
                catch
                {
                    return BadRequest();
                }
            }

            [Route("RegisterUser")]
            [HttpPost]
            public async Task<IActionResult> RegisterUser(string firstName, string lastName, string userName, string password)
            {
                if (String.IsNullOrWhiteSpace(userName))
                {
                    return BadRequest();
                }

                try
                {
                    MyUser newuser = new MyUser

                    {
                        FirstName = firstName,
                        LastName = lastName,
                        UserName = userName
                    };
                    await _userManager.CreateAsync(newuser, password);
                    await _context.SaveChangesAsync();

                    return Ok();

                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }
            }
        }
    }
}

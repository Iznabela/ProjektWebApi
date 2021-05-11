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
using ProjektWebApi.Models.V1;
using ProjektWebApi.Models.V2;

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

            [Route("GetGeoMessage")]
            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<GeoMessage>> GetGeoMessage(int? id)
            {
                try
                {
                    var geoMessage = await _context.GeoMessages.FindAsync(id);
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
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<ICollection<GeoMessage>>> GetGeoMessages()
            {
                try
                {
                    var geoMessages = await _context.GeoMessages.ToListAsync();
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
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> CreateGeoMessage(GeoMessage newGeoMessage)
            {
                if (String.IsNullOrWhiteSpace(newGeoMessage.Message))
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
            public async Task<ActionResult<GeoMessageV2>> GetGeoMessage(int? id)
            {
                try
                {
                    var geoMessage = await _context.GeoMessagesV2.ToListAsync();
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
            public async Task<ActionResult<ICollection<GeoMessageV2>>> GetGeoMessages(double? minLon, double? minLat, double? maxLon, double? maxLat)
            {
                try
                {
                    if (minLon == null || minLat == null || maxLon == null || maxLat == null)
                    {
                        var geoMessages = await _context.GeoMessagesV2.ToListAsync();
                        if (geoMessages != null)
                        {
                            return Ok(geoMessages);
                        }
                    }
                    else
                    {
                        var geoMessages = await _context.GeoMessagesV2.Where(g =>
                            g.Longitude >= minLon &&
                            g.Latitude >= minLat &&
                            g.Longitude <= maxLon &&
                            g.Latitude <= maxLat).ToListAsync();
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
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> CreateGeoMessage(GeoMessageV2 newGeoMessage)
            {
                if (String.IsNullOrWhiteSpace(newGeoMessage.Message.Title))
                {
                    return BadRequest();
                }

                try
                {
                    var geoMessage = new GeoMessageV2();
                    geoMessage.Message = newGeoMessage.Message;
                    geoMessage.Latitude = newGeoMessage.Latitude;
                    geoMessage.Longitude = newGeoMessage.Longitude;
                    await _context.GeoMessagesV2.AddAsync(newGeoMessage);
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

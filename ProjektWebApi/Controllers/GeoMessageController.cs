using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektWebApi.Data;
using ProjektWebApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektWebApi.Controllers
{
    namespace V1
    {
        [Route("api/v{version:apiVersion}/geo-comments")]
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
                public string Message { get; set; }
                public double Longitude { get; set; }
                public double Latitude { get; set; }
            }

            [Route("{id}")]
            [HttpGet]
            [SwaggerOperation(
                Summary = "Find a geo-message",
                Description = "Finding a geo-message based on ID"
                )]
            [SwaggerResponse(200, Description = "Geo-message based on ID was returned successfully")]
            [SwaggerResponse(400, Description = "Something went wrong with the request")]
            [SwaggerResponse(404, Description = "Could not find geo-message with supplied ID")]
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
            [SwaggerOperation(
                Summary = "Get multiple geo-messages",
                Description = "if no numbers are entered, all geo-messages are returned"
                )]
            [SwaggerResponse(200, "Geo-messages were returned successfully")]
            [SwaggerResponse(400, "Something went wrong with the request")]
            [SwaggerResponse(404, "Could not find geo-messages within range")]
            public async Task<ActionResult<ICollection<GeoMessage>>> GetGeoMessages(
                [FromQuery, SwaggerParameter("Minimum longitude", Required = false)] double? minLon,
                [FromQuery, SwaggerParameter("Maximum longitude", Required = false)] double? maxLon,
                [FromQuery, SwaggerParameter("Minimum Latitude", Required = false)] double? minLat,
                [FromQuery, SwaggerParameter("Maximum Latitude", Required = false)] double? maxLat)

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
            [HttpPost]
            [SwaggerOperation(
                Summary = "Create a geo-message",
                Description = "Creating a geo-message based on specified data - requires authentication"
                )]
            [SwaggerResponse(201, "Geo-message was created successfully", typeof(GeoMessage))]
            [SwaggerResponse(401, "Incorrect username or password")]
            [SwaggerResponse(400, "Something went wrong with the request")]
            public async Task<IActionResult> CreateGeoMessage(GeoMassageDTO newGeoMessage)
            {
                if (String.IsNullOrWhiteSpace(newGeoMessage.Message))
                {
                    return BadRequest();
                }

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    try
                    {
                        var geoMessage = new GeoMessage
                        {
                            Message = new Message
                            {
                                Title = "Unknown Title",
                                Author = user.FirstName + " " + user.LastName,
                                Body = newGeoMessage.Message
                            },
                            Latitude = newGeoMessage.Latitude,
                            Longitude = newGeoMessage.Longitude
                        };
                        await _context.GeoMessages.AddAsync(geoMessage);
                        await _context.SaveChangesAsync();
                        return CreatedAtAction(nameof(GetGeoMessage), new { id = geoMessage.Id }, geoMessage);
                    }
                    catch
                    {
                        return BadRequest();
                    }
                }
                return Unauthorized();
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
            [SwaggerOperation(
                Summary = "Find a geo-message",
                Description = "Finding a geo-message based on ID"
                )]
            [SwaggerResponse(200, Description = "Geo-message based on ID was returned successfully")]
            [SwaggerResponse(400, Description = "Something went wrong with the request")]
            [SwaggerResponse(404, Description = "Could not find geo-message with supplied ID")]
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

            [Route("GetGeoMessages")]
            [HttpGet]
            [SwaggerOperation(
                Summary = "Get multiple geo-messages",
                Description = "if no numbers are entered, all geo-messages are returned"
                )]
            [SwaggerResponse(200, "Geo-messages were returned successfully")]
            [SwaggerResponse(400, "Something went wrong with the request")]
            [SwaggerResponse(404, "Could not find geo-messages within range")]
            public async Task<ActionResult<ICollection<GeoMessage>>> GetGeoMessages(
                [FromQuery, SwaggerParameter("Minimum longitude", Required = false)] double? minLon,
                [FromQuery, SwaggerParameter("Maximum longitude", Required = false)] double? maxLon,
                [FromQuery, SwaggerParameter("Minimum Latitude", Required = false)] double? minLat,
                [FromQuery, SwaggerParameter("Maximum Latitude", Required = false)] double? maxLat)
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
            [SwaggerOperation(
                Summary = "Create a geo-message",
                Description = "Creating a geo-message based on specified data - requires authentication"
                )]
            [SwaggerResponse(201, "Geo-message was created successfully", typeof(GeoMessage))]
            [SwaggerResponse(401, "Incorrect username or password")]
            [SwaggerResponse(400, "Something went wrong with the request")]
            public async Task<IActionResult> CreateGeoMessage(GeoMessage newGeoMessage)
            {
                if (String.IsNullOrWhiteSpace(newGeoMessage.Message.Title))
                {
                    return BadRequest();
                }

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    try
                    {
                        var geoMessage = new GeoMessage
                        {
                            Message = new Message
                            {
                                Title = newGeoMessage.Message.Title,
                                Author = user.FirstName + " " + user.LastName,
                                Body = newGeoMessage.Message.Body
                            },
                            Latitude = newGeoMessage.Latitude,
                            Longitude = newGeoMessage.Longitude
                        };
                        await _context.GeoMessages.AddAsync(geoMessage);
                        await _context.SaveChangesAsync();
                        return CreatedAtAction(nameof(GetGeoMessage), new { id = geoMessage.Id }, geoMessage);
                    }
                    catch
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
        }
    }
}

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

namespace ProjektWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                GeoMessage geoMessage = await _context.GeoMessages.FindAsync(id);
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
                ICollection<GeoMessage> geoMessages = await _context.GeoMessages.ToListAsync();
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
                GeoMessage geoMessage = new GeoMessage();
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
                await _context.AddAsync(newuser);
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

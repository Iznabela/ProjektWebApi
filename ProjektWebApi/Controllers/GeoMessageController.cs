using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjektWebApi.Data;
using ProjektWebApi.Models;

namespace ProjektWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoMessageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GeoMessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GeoMessage>> GetGeoMessage(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                GeoMessage geoMessage = await _context.GeoMessages.FindAsync(id);
                return Ok(geoMessage);
            }
            catch
            {
                return NotFound();
            }
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

            GeoMessage geoMessage = new GeoMessage();
            geoMessage.Message = newGeoMessage.Message;
            geoMessage.Latitude = newGeoMessage.Latitude;
            geoMessage.Longitude = newGeoMessage.Longitude;

            try
            {
                await _context.GeoMessages.AddAsync(newGeoMessage);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetGeoMessage), new { id = geoMessage.Id }, geoMessage);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}

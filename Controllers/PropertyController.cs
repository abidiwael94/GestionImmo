using GestionImmo.Data;
using GestionImmo.Models.Entities;
using GestionImmo.Models.DTO;
using GestionImmo.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionImmo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PropertyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Property
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            var properties = await _context.Properties
                .Include(p => p.User)
                .Include(p => p.Photos)
                .Include(p => p.Visits)
                .Include(p => p.Favorites)
                .ToListAsync();

            return Ok(properties);
        }

        // GET: api/Property/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(Guid id)
        {
            var property = await _context.Properties
                .Include(p => p.User)
                .Include(p => p.Photos)
                .Include(p => p.Visits)
                .Include(p => p.Favorites)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
                return NotFound();

            return Ok(property);
        }

        // POST: api/Property
        [HttpPost]
        public async Task<ActionResult<Property>> CreateProperty(PropertyCreateDto dto)
        {
            var property = new Property
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                Address = dto.Address,
                Status = dto.Status == 0 ? PropertyStatut.AVAILABLE : dto.Status,
                UserId = dto.UserId
            };
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
        }


        // PUT: api/Property/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] PropertyUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("L'ID ne correspond pas.");

            var property = await _context.Properties.FindAsync(id);
            if (property == null)
                return NotFound();

            // Mettre à jour les champs
            property.Description = dto.Description;
            property.Address = dto.Address;
            property.Status = dto.Status;
            property.UserId = dto.UserId;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/Property/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
                return NotFound();

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

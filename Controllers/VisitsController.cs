using GestionImmo.Data;
using GestionImmo.Models.DTO;
using GestionImmo.Models.Entities;
using GestionImmo.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GestionImmo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VisitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Visits
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var visits = await _context.Visits
                .Include(v => v.Property)
                .Include(v => v.User)
                .ToListAsync();

            return Ok(visits);
        }

        // GET: api/Visits/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var visit = await _context.Visits
                .Include(v => v.Property)
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visit == null) return NotFound();

            return Ok(visit);
        }

        // POST: api/Visits
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VisitDTO dto)
        {
            if (!Enum.TryParse<VisitStatus>(dto.Status, out var status))
            {
                return BadRequest("Invalid status.");
            }

            var visit = new Visit
            {
                Id = Guid.NewGuid(),
                PropertyId = dto.PropertyId,
                UserId = dto.UserId,
                Status = status,
                VisitDate = dto.VisitDate
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = visit.Id }, visit);
        }

        // PUT: api/Visits/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] VisitDTO dto)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null) return NotFound();

            if (!Enum.TryParse<VisitStatus>(dto.Status, out var status))
            {
                return BadRequest("Invalid status.");
            }

            visit.PropertyId = dto.PropertyId;
            visit.UserId = dto.UserId;
            visit.Status = status;
            visit.VisitDate = dto.VisitDate;

            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Visits/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null) return NotFound();

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Visits/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var visits = await _context.Visits
                .Where(v => v.UserId == userId)
                .Include(v => v.Property)
                .ToListAsync();

            return Ok(visits);
        }

        // GET: api/Visits/check-availability?propertyId={propertyId}&date=yyyy-MM-dd
        [HttpGet("check-availability")]
        public async Task<IActionResult> CheckDateAvailability(Guid propertyId, DateTime date)
        {
            var isAvailable = !await _context.Visits
                .AnyAsync(v => v.PropertyId == propertyId &&
                               v.VisitDate.Date == date.Date);

            return Ok(new { available = isAvailable });
        }

        // GET: api/Visits/upcoming/{propertyId}
        [HttpGet("upcoming/{propertyId}")]
        public IActionResult GetUpcomingVisits(Guid propertyId)
        {
            var visits = _context.Visits
                .Where(v => v.PropertyId == propertyId &&
                            v.VisitDate >= DateTime.Today)
                .OrderBy(v => v.VisitDate)
                .Select(v => new
                {
                    v.Id,
                    v.VisitDate,
                    v.Status,
                    v.UserId
                })
                .ToList();

            return Ok(visits);
        }

        // GET: api/Visits/count-by-date/{propertyId}
        [HttpGet("count-by-date/{propertyId}")]
        public IActionResult GetVisitCountByDate(Guid propertyId)
        {
            var data = _context.Visits
                .Where(v => v.PropertyId == propertyId)
                .GroupBy(v => v.VisitDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            return Ok(data);
        }
    }
}

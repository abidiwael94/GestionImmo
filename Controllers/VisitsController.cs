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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var visits = await _context.Visits
                .Include(v => v.Property)
                .Include(v => v.User)
                .ToListAsync();

            return Ok(visits);
        }

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

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VisitDTO dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return BadRequest("Invalid user.");

            if (user.Role != Role.CLIENT)
                return BadRequest("Only clients can request visits.");

            var visit = new Visit
            {
                Id = Guid.NewGuid(),
                PropertyId = dto.PropertyId,
                UserId = dto.UserId,
                VisitDate = dto.VisitDate,
                Status = VisitStatus.WAITING // Only WAITING allowed for clients
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = visit.Id }, visit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] VisitDTO dto)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null) return NotFound();

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return BadRequest("Invalid user.");

            if (!Enum.TryParse<VisitStatus>(dto.Status, out var newStatus))
                return BadRequest("Invalid status.");

            // CLIENT cannot update anything
            if (user.Role == Role.CLIENT)
                return Forbid("Clients are not allowed to modify visits.");

            // AGENT can update status to CONFIRMED, REFUSED, REPORTED
            if (user.Role == Role.AGENT)
            {
                if (newStatus != VisitStatus.CONFIRMED &&
                    newStatus != VisitStatus.REFUSED &&
                    newStatus != VisitStatus.REPORTED)
                {
                    return BadRequest("Agents can only CONFIRM, REFUSE or REPORT visits.");
                }

                visit.Status = newStatus;
                visit.VisitDate = dto.VisitDate;
            }

            visit.PropertyId = dto.PropertyId;
            visit.UserId = dto.UserId;

            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null) return NotFound();

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var visits = await _context.Visits
                .Where(v => v.UserId == userId)
                .Include(v => v.Property)
                .ToListAsync();

            return Ok(visits);
        }

        [HttpGet("check-availability")]
        public async Task<IActionResult> CheckDateAvailability(Guid propertyId, DateTime date)
        {
            var isAvailable = !await _context.Visits
                .AnyAsync(v => v.PropertyId == propertyId &&
                               v.VisitDate.Date == date.Date);

            return Ok(new { available = isAvailable });
        }

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

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingVisits()
        {
            var visits = await _context.Visits
                .Where(v => v.Status == VisitStatus.WAITING)
                .Include(v => v.Property)
                .Include(v => v.User)
                .ToListAsync();

            return Ok(visits);
        }
    }
}

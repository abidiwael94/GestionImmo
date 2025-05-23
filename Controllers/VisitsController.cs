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
        public  IActionResult GetAll()
        {
            var visits = _context.Visits
                .Include(v => v.Property)
                .Include(v => v.User)
                .ToList();

            return Ok(visits);
        }

        [HttpGet("{id}")]
        public  IActionResult GetById(Guid id)
        {
            var visit = _context.Visits
                .Include(v => v.Property)
                .Include(v => v.User)
                .FirstOrDefault(v => v.Id == id);

            if (visit == null) return NotFound();

            return Ok(visit);
        }

        [HttpPost]
        public IActionResult Add([FromBody] VisitDTO dto)
        {
            var user = _context.Users.Find(dto.UserId);
            if (user == null) return BadRequest("Invalid user.");

            if (user.Role != Role.CLIENT)
                return BadRequest("Only clients can request visits.");

            var visit = new Visit
            {
                Id = Guid.NewGuid(),
                PropertyId = dto.PropertyId,
                UserId = dto.UserId,
                VisitDate = dto.VisitDate,
                Status = VisitStatus.WAITING 
            };

            _context.Visits.Add(visit);
            _context.SaveChanges();

            return Ok(visit);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(Guid id, [FromBody] VisitDTO dto)
        {
            var visit =  _context.Visits.Find(id);
            if (visit == null) return NotFound();

            var user =  _context.Users.Find(dto.UserId);
            if (user == null) return BadRequest("Invalid user.");

            if (!Enum.TryParse<VisitStatus>(dto.Status, out var newStatus))
                return BadRequest("Invalid status.");

            if (user.Role == Role.CLIENT)
                return Forbid("Clients are not allowed to modify visits.");

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
             _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public  IActionResult Delete(Guid id)
        {
            var visit =  _context.Visits.Find(id);
            if (visit == null) return NotFound();

            _context.Visits.Remove(visit);
             _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public  IActionResult GetByUserId(Guid userId)
        {
            var visits =  _context.Visits
                .Where(v => v.UserId == userId)
                .Include(v => v.Property)
                .ToList();

            return Ok(visits);
        }

        [HttpGet("check-availability")]
        public IActionResult CheckDateAvailability(Guid propertyId, DateTime date)
        {
            var isAvailable = ! _context.Visits
                .Any(v => v.PropertyId == propertyId &&
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
        public IActionResult GetPendingVisits()
        {
            var visits =  _context.Visits
                .Where(v => v.Status == VisitStatus.WAITING)
                .Include(v => v.Property)
                .Include(v => v.User)
                .ToList();

            return Ok(visits);
        }
    }
}

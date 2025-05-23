using GestionImmo.Data;
using GestionImmo.Models.Entities;
using GestionImmo.Models.Dtos;
using GestionImmo.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionImmo.Models.DTO;

namespace GestionImmo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            var result = users.Select(u => new UserDto
            {
                Email = u.email,
                Password = u.password // Pour sécurité, tu peux choisir de ne pas retourner ça
            });

            return Ok(result);
        }

        // GET: api/user/{email}
        [HttpGet("{email}")]
        public async Task<ActionResult<UserDto>> GetByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null) return NotFound();

            var result = new UserDto
            {
                Email = user.email,
                Password = user.password
            };

            return Ok(result);
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<UserDto>> Add([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(u => u.email == dto.Email))
                return Conflict("Un utilisateur avec cet email existe déjà.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                email = dto.Email,
                password = HashPassword(dto.Password),
                FullName = dto.FullName ?? "Nouveau Utilisateur",
                address = dto.Address ?? "Adresse par défaut",
                phone = dto.Phone ?? "00000000",
                
                Properties = new List<Property>(),
                Visits = new List<Visit>(),
                Favorites = new List<Favorite>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByEmail), new { email = user.email }, dto);
        }


        // PUT: api/user/{email}
        [HttpPut("{email}")]
        public async Task<IActionResult> Update(string email, [FromBody] UserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null) return NotFound();

            user.password = HashPassword(dto.Password);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/user/{email}
        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

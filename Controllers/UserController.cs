using GestionImmo.Data;
using GestionImmo.Models.DTO;
using GestionImmo.Models.Dtos;
using GestionImmo.Models.Entities;
using GestionImmo.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        // 🔐 Route protégée par JWT
        [Authorize]
        [HttpGet("profil")]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
                return Unauthorized();

            var user = await _context.Users
                .Include(f => f.Favorites)
                .Include(f => f.Visits)
                .Include(f => f.Properties)
                .FirstOrDefaultAsync(u => u.email == email);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .Include(f => f.Favorites)
                .Include(f => f.Visits)
                .Include(f => f.Properties)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _context.Users
                .Include(f => f.Favorites)
                .Include(f => f.Visits)
                .Include(f => f.Properties)
                .FirstOrDefaultAsync(u => u.email == email);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                email = dto.Email,
                password = HashPassword(dto.Password),
                FullName = dto.FullName,
                address = dto.Address,
                phone = dto.Phone,
                Role = Role.CLIENT,
                Properties = new List<Property>(),
                Visits = new List<Visit>(),
                Favorites = new List<Favorite>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> Update(string email, [FromBody] UserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null) return NotFound();

            user.email = dto.Email;
            user.password = HashPassword(dto.Password);
            user.FullName = dto.FullName ?? user.FullName;
            user.phone = dto.Phone ?? user.phone;
            user.address = dto.Address ?? user.address;
            user.Role = dto.Role ?? user.Role;

            await _context.SaveChangesAsync();

            return NoContent();
        }

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

using GestionImmo.Data;
using GestionImmo.Models.Dtos;
using GestionImmo.Models.Entities;
using GestionImmo.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestionImmo.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.email == dto.Email))
                return BadRequest("Email déjà utilisé");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                email = dto.Email,
                password = HashPassword(dto.Password),
                address = dto.Address,
                Role = Role.CLIENT,
                phone = dto.Phone
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Utilisateur enregistré");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.password))
                return Unauthorized("Identifiants invalides");

            return Ok("Connexion réussie");
        }
    }
}

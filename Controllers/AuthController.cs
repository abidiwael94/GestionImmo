using GestionImmo.Data;
using GestionImmo.Models.Dtos;
using GestionImmo.Models.Entities;
using GestionImmo.Models.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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

        private readonly IEmailSender _emailSender;

        public AuthController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
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

            // Send welcome email
            var subject = "Bienvenue chez GestionImmo !";
            var body = $"<h1>Bonjour {user.FullName},</h1><p>Merci pour votre inscription.</p>";
            await _emailSender.SendEmailAsync(user.email, subject, body);

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

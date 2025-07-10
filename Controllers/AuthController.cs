using GestionImmo.Data;
using GestionImmo.Models.DTO;
using GestionImmo.Models.Dtos;
using GestionImmo.Models.Entities;
using GestionImmo.Models.Enum;
using GestionImmo.Services;
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
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, IEmailSender emailSender, JwtService jwtService)
        {
            _context = context;
            _emailSender = emailSender;
            _jwtService = jwtService;
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

        //  REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.email == dto.Email))
            {
                return BadRequest(new { message = "Email déjà utilisé" });
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                email = dto.Email,
                password = HashPassword(dto.Password),
                address = dto.Address,
                Role = dto.Role ?? Role.CLIENT,
                phone = dto.Phone
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var subject = "Bienvenue chez GestionImmo !";
            var body = $"<h1>Bonjour {user.FullName},</h1><p>Merci pour votre inscription.</p>";
            await _emailSender.SendEmailAsync(user.email, subject, body);

            //  Répond avec un JSON
            return Ok(new
            {
                message = "Utilisateur enregistré avec succès",
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.email,
                    user.address,
                    user.phone,
                    role = user.Role.ToString()
                }
            });
        }

        //  LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.password))
            {
                return Unauthorized(new { message = "Identifiants invalides" });
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                message = "Connexion réussie",
                token,
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.email,
                    user.address,
                    user.phone,
                    role = user.Role.ToString()
                }
            });
        }
    }
}

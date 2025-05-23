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
using Microsoft.AspNetCore.Http.HttpResults;

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

        [HttpGet]
        public IActionResult GetAll()
        {
            var users =  _context.Users
                .Include(f => f.Favorites)
                .Include(f => f.Visits)
                .Include(f => f.Properties)
                .ToList();
            return Ok(users);
        }

        [HttpGet("{email}")]
        public IActionResult GetByEmail(string email)
        {
            var user = _context.Users
                .Include (f => f.Favorites)
                .Include(f => f.Visits)
                .Include(f => f.Properties)
                .FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add(UserDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                email = dto.Email,
                password = HashPassword(dto.Password),
                FullName = dto.FullName,
                address = dto.Address,
                phone = dto.Phone,
                
                Properties = new List<Property>(),
                Visits = new List<Visit>(),
                Favorites = new List<Favorite>()
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }


        [HttpPut("{email}")]
        public  IActionResult Update(string email, [FromBody] UserDto dto)
        {
            var user =  _context.Users.FirstOrDefault(u => u.email == email);
            if (user == null) return NotFound();

            
            user.email = dto.Email;
            user.password = HashPassword(dto.Password);
            user.FullName = dto.FullName ?? user.FullName;
            user.phone = dto.Phone ?? user.phone;
            user.address = dto.Address ?? user.address;
            user.Role = dto.Role ?? user.Role;

            _context.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{email}")]
        public IActionResult Delete(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == email);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

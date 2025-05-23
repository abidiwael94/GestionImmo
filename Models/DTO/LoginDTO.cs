using System.ComponentModel.DataAnnotations;

namespace GestionImmo.Models.DTO
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

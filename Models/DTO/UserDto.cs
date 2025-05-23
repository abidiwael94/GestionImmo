using GestionImmo.Models.Enum;
using System.ComponentModel.DataAnnotations;
namespace GestionImmo.Models.Dtos;


public class UserDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public Role? Role { get; set; }  

    public string? FullName { get; set; } = "Nouveau Utilisateur";

    public string? Address { get; set; } = "Adresse par défaut";

    public string? Phone { get; set; } = "00000000";
}

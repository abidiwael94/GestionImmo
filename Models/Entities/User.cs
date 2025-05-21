using GestionImmo.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionImmo.Models.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public string phone { get; set; }

        [InverseProperty("User")]
        public ICollection<Property> Properties { get; set; }

        [InverseProperty("User")]
        public ICollection<Visit> Visits { get; set; }

        [InverseProperty("CreatedBy")]
        public ICollection<Favorite> Favorites { get; set; }
    }
}



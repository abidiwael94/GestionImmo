using GestionImmo.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GestionImmo.Models.Enum;

namespace GestionImmo.Models.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

       
        public string FullName { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        
        public string address { get; set; }


        public Role Role { get; set; }

        
        public string phone { get; set; }

        [InverseProperty("User")]
        public ICollection<Property> Properties { get; set; }

        [InverseProperty("User")]
        public ICollection<Visit> Visits { get; set; }

        [InverseProperty("CreatedBy")]
        public ICollection<Favorite> Favorites { get; set; }
    }
}



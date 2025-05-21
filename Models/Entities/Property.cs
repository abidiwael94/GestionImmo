using GestionImmo.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GestionImmo.Models.Entities
{
    public class Property
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public string Address { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Properties")]
        public User User { get; set; }

        [InverseProperty("Property")]
        public ICollection<Photo> Photos { get; set; }

        [InverseProperty("Property")]
        public ICollection<Visit> Visits { get; set; }

        [InverseProperty("Property")]
        public ICollection<Favorite> Favorites { get; set; }
    }
}
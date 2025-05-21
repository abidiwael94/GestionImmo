using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionImmo.Models.Entities
{
    public class Favorite
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        [InverseProperty("Favorites")]
        public Property Property { get; set; }

        public Guid CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        [InverseProperty("Favorites")]
        public User CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}

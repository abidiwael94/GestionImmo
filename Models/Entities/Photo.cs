using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionImmo.Models.Entities
{
    public class Photo
    {
        [Key]
        public Guid Id { get; set; } 

        [Required]
        public string Path { get; set; }

        [Required]
        public Guid PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        [InverseProperty("Photos")]
        public Property Property { get; set; }
    }
}

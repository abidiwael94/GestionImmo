using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GestionImmo.Models.Enum;

namespace GestionImmo.Models.Entities
{
    public class Visit
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PropertyId { get; set; }


        [Required]
        public VisitStatus Status { get; set; }

        [ForeignKey("PropertyId")]
        [InverseProperty("Visits")]
        public Property Property { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Visits")]
        public User User { get; set; }
    }
}

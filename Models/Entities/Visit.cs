using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionImmo.Models.Entities
{
    public class Visit
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        [InverseProperty("Visits")]
        public Property Property { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Visits")]
        public User User { get; set; }
    }
}

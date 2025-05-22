using System.ComponentModel.DataAnnotations;

namespace GestionImmo.Models.DTO
{
    public class VisitDTO
    {
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime VisitDate { get; set; }


    }
}

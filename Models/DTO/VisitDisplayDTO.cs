namespace GestionImmo.Models.DTO
{
    public class VisitDisplayDTO
    {
        public Guid Id { get; set; }
        public string PropertyTitle { get; set; }
        public string UserFullName { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}

namespace GestionImmo.Models.DTO
{
    public class FavoriteDto
    {
        public Guid PropertyId { get; set; }

        public Guid CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

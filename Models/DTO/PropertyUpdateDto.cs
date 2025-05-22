using System;
using GestionImmo.Models.Enum;

namespace GestionImmo.Models.DTO
{
    public class PropertyUpdateDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public PropertyStatut Status { get; set; }
        public Guid UserId { get; set; }
    }
}

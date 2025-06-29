using System;
using System.ComponentModel.DataAnnotations;
using GestionImmo.Models.Enum;

namespace GestionImmo.Models.DTO
{
    public class PropertyCreateDto
    {
        [Required(ErrorMessage = "La description est obligatoire.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "L'adresse est obligatoire.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Le statut est obligatoire.")]
        public PropertyStatut Status { get; set; }

        [Required(ErrorMessage = "L'identifiant de l'utilisateur est obligatoire.")]
        public Guid UserId { get; set; }

        // --- Predictive Features ---

        [Range(0, 100)]
        public int Bedrooms { get; set; }

        [Range(0, 100)]
        public int Bathrooms { get; set; }

        public double SquareFeet { get; set; }

        public double LotSize { get; set; }

        public int YearBuilt { get; set; }

        public PropertyType PropertyType { get; set; }

        public int Floor { get; set; }

        public int TotalFloors { get; set; }

        public bool HasGarage { get; set; }

        public int GarageSpaces { get; set; }

        public bool HasBasement { get; set; }

        public bool HasPool { get; set; }

        public bool HasElevator { get; set; }

        public bool Furnished { get; set; }

        public PropertyCondition Condition { get; set; }

        public HeatingType HeatingType { get; set; }

        public CoolingType CoolingType { get; set; }

        public string ZipCode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime ListingDate { get; set; }

        public decimal? EstimatedPrice { get; set; } // Optional
    }
}

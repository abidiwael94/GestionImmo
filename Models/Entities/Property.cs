﻿using GestionImmo.Models.Entities;
using GestionImmo.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GestionImmo.Models.Entities
{
    public class Property
    {
        internal string Title;

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public PropertyStatut Status { get; set; }

        [Required]
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

        // --- Predictive Features ---

        public int Bedrooms { get; set; }

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

        public decimal? EstimatedPrice { get; set; } 
    }

}
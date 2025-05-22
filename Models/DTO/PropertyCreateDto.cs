using System;
using System.ComponentModel.DataAnnotations;
using GestionImmo.Models.Enum;

namespace GestionImmo.Models.DTO
{
	public class PropertyCreateDto
	{
		[Required(ErrorMessage = "La description est obligatoire.")]
		public string Description { get; set; }

		
		public string Address { get; set; }

		
		public PropertyStatut Status { get; set; } 

		[Required(ErrorMessage = "L'identifiant de l'utilisateur est obligatoire.")]
		public Guid UserId { get; set; }
	}
}

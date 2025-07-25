    using GestionImmo.Data;
    using GestionImmo.Models.DTO;
    using GestionImmo.Models.Entities;
    using GestionImmo.Models.Enum;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
using System.Text.Json;
    using System.Threading.Tasks;

    namespace GestionImmo.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class PropertyController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public PropertyController(ApplicationDbContext context)
            {
                _context = context;
            }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetProperties()
        {
            var properties = await _context.Properties
                .Include(p => p.User)
                .Include(p => p.Photos)
                .ToListAsync();

            var dtoList = properties.Select(p => new PropertyDto
            {
                Id = p.Id,
                Description = p.Description,
                Address = p.Address,
                Status = p.Status,
                UserId = p.UserId,
                UserFullName = p.User.FullName,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                SquareFeet = p.SquareFeet,
                LotSize = p.LotSize,
                YearBuilt = p.YearBuilt,
                PropertyType = p.PropertyType,
                Floor = p.Floor,
                TotalFloors = p.TotalFloors,
                HasGarage = p.HasGarage,
                GarageSpaces = p.GarageSpaces,
                HasBasement = p.HasBasement,
                HasPool = p.HasPool,
                HasElevator = p.HasElevator,
                Furnished = p.Furnished,
                Condition = p.Condition,
                HeatingType = p.HeatingType,
                CoolingType = p.CoolingType,
                ZipCode = p.ZipCode,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                ListingDate = p.ListingDate,
                EstimatedPrice = p.EstimatedPrice,
                PhotoPaths = p.Photos.Select(photo => new PhotoDto
                {
                    Id = photo.Id,
                    Path = photo.Path
                }).ToList()
            });

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
            public async Task<ActionResult<Property>> GetProperty(Guid id)
            {
                var property = await _context.Properties
                    .Include(p => p.User)
                    .Include(p => p.Photos)
                    .Include(p => p.Visits)
                    .Include(p => p.Favorites)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (property == null)
                    return NotFound();

                return Ok(property);
            }

            [HttpPost]
            public async Task<ActionResult<Property>> CreateProperty(PropertyCreateDto dto)
            {
                var user = await _context.Users.FindAsync(dto.UserId);
                if (user == null)
                    return BadRequest("L'ID de user ne correspond pas.");

                if (user.Role == Role.CLIENT)
                    return BadRequest("Utilisateur n'est pas autoris�");

            decimal predictedPrice = await PredictEstimatedPriceAsync(dto);


            var property = new Property
                {
                    Id = Guid.NewGuid(),
                    Description = dto.Description,
                    Address = dto.Address,
                    Status = dto.Status == 0 ? PropertyStatut.AVAILABLE : dto.Status,
                    UserId = dto.UserId,

                    // New fields
                    Bedrooms = dto.Bedrooms,
                    Bathrooms = dto.Bathrooms,
                    SquareFeet = dto.SquareFeet,
                    LotSize = dto.LotSize,
                    YearBuilt = dto.YearBuilt,
                    PropertyType = dto.PropertyType,
                    Floor = dto.Floor,
                    TotalFloors = dto.TotalFloors,
                    HasGarage = dto.HasGarage,
                    GarageSpaces = dto.GarageSpaces,
                    HasBasement = dto.HasBasement,
                    HasPool = dto.HasPool,
                    HasElevator = dto.HasElevator,
                    Furnished = dto.Furnished,
                    Condition = dto.Condition,
                    HeatingType = dto.HeatingType,
                    CoolingType = dto.CoolingType,
                    ZipCode = dto.ZipCode,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    ListingDate = dto.ListingDate,
                    EstimatedPrice = predictedPrice
            };

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
            }



        /// prediction 
        /// 
        /// 
        /// 
        /// 


        private async Task<decimal> PredictEstimatedPriceAsync(PropertyCreateDto dto)
        {
            using var httpClient = new HttpClient();

            var payload = new
            {
                Bedrooms = dto.Bedrooms,
                Bathrooms = dto.Bathrooms,
                SquareFeet = dto.SquareFeet,
                LotSize = dto.LotSize,
                YearBuilt = dto.YearBuilt,
                Floor = dto.Floor,
                TotalFloors = dto.TotalFloors,
                HasGarage = dto.HasGarage,
                GarageSpaces = dto.GarageSpaces,
                HasBasement = dto.HasBasement,
                HasPool = dto.HasPool,
                HasElevator = dto.HasElevator,
                Furnished = dto.Furnished,
                ZipCode = dto.ZipCode
            };
            var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Sending payload to prediction API:\n" + jsonPayload);
            var response = await httpClient.PostAsJsonAsync("http://localhost:5000/predict", payload);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Prediction API failed");

            var result = await response.Content.ReadFromJsonAsync<PredictionResponse>();
            return result.EstimatedPrice;
        }

        private class PredictionResponse
        {
            public decimal EstimatedPrice { get; set; }

            // In case Flask uses snake_case
            [System.Text.Json.Serialization.JsonPropertyName("estimated_price")]
            public decimal EstimatedPriceSnakeCase
            {
                set => EstimatedPrice = value;
            }
        }



        ///

        //add image
        [HttpPost("{propertyId}/upload-photo")]
        public async Task<IActionResult> UploadPhoto(Guid propertyId, IFormFile file)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null)
                return NotFound("Property not found.");

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Local storage path: /wwwroot/uploads/{propertyId}
            var folderPath = Path.Combine("wwwroot", "uploads", propertyId.ToString());
            Directory.CreateDirectory(folderPath); // Ensure directory exists

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo
            {
                Id = Guid.NewGuid(),
                Path = Path.Combine("uploads", propertyId.ToString(), uniqueFileName).Replace("\\", "/"),
                PropertyId = propertyId
            };

            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            return Ok(new { photo.Id, photo.Path });
        }


        [HttpGet("photo/{photoId}")]
        public async Task<IActionResult> GetImage(Guid photoId)
        {
            var photo = await _context.Photos.FindAsync(photoId);
            if (photo == null)
                return NotFound("Photo not found.");

            var fullPath = Path.Combine("wwwroot", photo.Path);
            if (!System.IO.File.Exists(fullPath))
                return NotFound("File not found on disk.");

            var fileExtension = Path.GetExtension(fullPath).ToLowerInvariant();
            var contentType = fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            return File(fileBytes, contentType);
        }

        [HttpGet("{propertyId}/photos")]
        public async Task<IActionResult> GetPhotosForProperty(Guid propertyId)
        {
            var photos = await _context.Photos
                .Where(p => p.PropertyId == propertyId)
                .ToListAsync();

            var photoPaths = photos.Select(p => p.Path).ToList();
            return Ok(photoPaths);
        }



        [HttpPut("{id}")]
            public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] PropertyUpdateDto dto)
            {
                if (id != dto.Id)
                    return BadRequest("L'ID ne correspond pas.");

                var property = await _context.Properties.FindAsync(id);
                if (property == null)
                    return NotFound();

                // Basic fields
                property.Description = dto.Description;
                property.Address = dto.Address;
                property.Status = dto.Status;
                property.UserId = dto.UserId;

                // Updated fields
                property.Bedrooms = dto.Bedrooms;
                property.Bathrooms = dto.Bathrooms;
                property.SquareFeet = dto.SquareFeet;
                property.LotSize = dto.LotSize;
                property.YearBuilt = dto.YearBuilt;
                property.PropertyType = dto.PropertyType;
                property.Floor = dto.Floor;
                property.TotalFloors = dto.TotalFloors;
                property.HasGarage = dto.HasGarage;
                property.GarageSpaces = dto.GarageSpaces;
                property.HasBasement = dto.HasBasement;
                property.HasPool = dto.HasPool;
                property.HasElevator = dto.HasElevator;
                property.Furnished = dto.Furnished;
                property.Condition = dto.Condition;
                property.HeatingType = dto.HeatingType;
                property.CoolingType = dto.CoolingType;
                property.ZipCode = dto.ZipCode;
                property.Latitude = dto.Latitude;
                property.Longitude = dto.Longitude;
                property.ListingDate = dto.ListingDate;
                property.EstimatedPrice = dto.EstimatedPrice;

                await _context.SaveChangesAsync();

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteProperty(Guid id)
            {
                var property = await _context.Properties.FindAsync(id);
                if (property == null)
                    return NotFound();

                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

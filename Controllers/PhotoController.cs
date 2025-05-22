using GestionImmo.Data;
using GestionImmo.Models.DTO;
using GestionImmo.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionImmo.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public PhotoController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetPhotos()
        {
            var photos = dbContext.Photos.ToList();
            return Ok(photos);
        }

        [HttpGet("{id}")]
        public IActionResult GetPhotoById(Guid id)
        {
            var photo = dbContext.Photos.Find(id);
            if (photo == null)
            {
                return NotFound();
            }
            return Ok(photo);
        }

        [HttpGet("by-property/{propertyId}")]
        public IActionResult GetPhotosByProperty(Guid propertyId)
        {
            var photos = dbContext.Photos
                .Where(p => p.PropertyId == propertyId)
                .ToList();

            return Ok(photos);
        }

        [HttpPost]
        public IActionResult AddPhoto(PhotoDto photoDto)
        {
            var newPhoto = new Photo
            {
                Path = photoDto.Path,
                PropertyId = photoDto.PropertyId
            };

            dbContext.Photos.Add(newPhoto);
            dbContext.SaveChanges();

            return Ok(newPhoto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePhoto(Guid id, PhotoDto photoDto)
        {
            var existingPhoto = dbContext.Photos.Find(id);
            if (existingPhoto == null)
            {
                return NotFound();
            }

            existingPhoto.Path = photoDto.Path;
            existingPhoto.PropertyId = photoDto.PropertyId;

            dbContext.SaveChanges();

            return Ok(existingPhoto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePhoto(Guid id)
        {
            var photo = dbContext.Photos.Find(id);
            if (photo == null)
            {
                return NotFound();
            }

            dbContext.Photos.Remove(photo);
            dbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet("count")]
        public IActionResult GetPhotoCount()
        {
            int count = dbContext.Photos.Count();
            return Ok(count);
        }
    }
}

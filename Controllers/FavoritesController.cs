using GestionImmo.Data;
using GestionImmo.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionImmo.Models.Entities;

namespace GestionImmo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public FavoritesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetFavorites()
        {
            var favorites = dbContext.Favorites
                .Include(f => f.Property)
                .Include(f => f.CreatedBy)
                .ToList();

            return Ok(favorites);
        }
        [HttpGet("by-user/{userId}")]
        public IActionResult GetFavoritesByUser(Guid userId)
        {
            var favorites = dbContext.Favorites
                .Where(f => f.CreatedById == userId)
                .Include(f => f.Property)
                .ToList();

            return Ok(favorites);
        }

        [HttpGet("ordered-by-date")]
        public IActionResult GetFavoritesOrderedByDate()
        {
            var favorites = dbContext.Favorites
                .OrderByDescending(f => f.CreatedAt)
                .Include(f => f.Property)
                .ToList();

            return Ok(favorites);
        }

        [HttpPost]
        public IActionResult AddFavorite(FavoriteDto favorite)
        {
            var Newfavorite = new Favorite
            {
                PropertyId = favorite.PropertyId,
                CreatedById = favorite.CreatedById,
                CreatedAt = DateTime.UtcNow
            };
            dbContext.Favorites.Add(Newfavorite);
            dbContext.SaveChanges();

            var resFavorite = dbContext.Favorites
               .Include(f => f.Property)
               .Include(f => f.CreatedBy)
               .FirstOrDefault(f => f.Id == Newfavorite.Id);

            return Ok(resFavorite);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFavorite(Guid id, FavoriteDto favorite)
        {
            var existingFavorite = dbContext.Favorites.Find(id);
            if (existingFavorite == null)
            {
                return NotFound();
            }
            existingFavorite.PropertyId = favorite.PropertyId;
            existingFavorite.CreatedById = favorite.CreatedById;
            existingFavorite.CreatedAt = DateTime.UtcNow;
            dbContext.SaveChanges();

            var updatedFavorite = dbContext.Favorites
                .Include(f => f.Property)
                .Include(f => f.CreatedBy)
                .FirstOrDefault(f => f.Id == existingFavorite.Id);

            return Ok(updatedFavorite);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFavorite(Guid id)
        {
            var favorite = dbContext.Favorites.Find(id);
            if (favorite == null)
            {
                return NotFound();
            }
            dbContext.Favorites.Remove(favorite);
            dbContext.SaveChanges();
            return NoContent();
        }

        [HttpGet("has-favorite/{userId}")]
        public IActionResult HasFavorite(Guid userId)
        {
            bool hasFavorite = dbContext.Favorites.Any(f => f.CreatedById == userId);
            return Ok(hasFavorite);
        }

        [HttpGet("count")]
        public IActionResult GetFavoritesCount()
        {
            int count = dbContext.Favorites.Count();
            return Ok(count);
        }

    }
}
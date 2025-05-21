using GestionImmo.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionImmo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public FavoritesController(ApplicationDbContext dbContext) {
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
    }
}

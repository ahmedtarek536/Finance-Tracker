using Finance_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finance_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var categories = await _context.Categories
                                          .Where(c => c.UserId == userId)
                                          .ToListAsync();
            return Ok(categories); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(string title, int userId)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest("Title is required.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var category = new Category
            {
                Title = title,
                UserId = userId,
                User = user
            };

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return Ok(category); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the category.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return Ok("Category deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the category.");
            }
        }



    }
}

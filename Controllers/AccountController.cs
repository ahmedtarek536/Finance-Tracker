using Finance_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finance_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var accounts = await _context.Accounts
                                        .Where(a => a.UserId == userId)
                                        .ToListAsync();
            return Ok(accounts); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(string title, int userId)
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

            var account = new Account
            {
                Title = title,
                UserId = userId,
                User = user
            };

            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return Ok(account); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the account.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            try
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
                return Ok("Account deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the account.");
            }
        }




    }
}

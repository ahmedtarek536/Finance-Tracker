using Finance_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Finance_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var transactions = await _context.Transactions
                                             .Where(t => t.UserId == userId)
                                             .ToListAsync();
            return Ok(transactions); 
        }


        [HttpPost]
        public async Task<IActionResult> CreateTransaction(string title, string payee, decimal amount, int accountId, int categoryId, int userId)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(payee))
            {
                return BadRequest("Title and Payee are required.");
            }

            if (amount <= 0)
            {
                return BadRequest("Amount must be a positive number.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId);
            if (account == null)
            {
                return NotFound("Account not found.");
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var transaction = new Transaction
            {
                Title = title,
                Payee = payee,
                Amount = amount,
                AccountId = accountId,
                CategoryId = categoryId,
                UserId = userId,
                User = user,
                Account = account,
                Category = category,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            try
            {
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return Ok(transaction); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the transaction.");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Services.TransactionDto updatedTransaction)
        {
            if (updatedTransaction == null || string.IsNullOrEmpty(updatedTransaction.Title) || string.IsNullOrEmpty(updatedTransaction.Payee))
            {
                return BadRequest("Invalid input. Title and Payee are required.");
            }

            if (updatedTransaction.Amount <= 0)
            {
                return BadRequest("Amount must be a positive number.");
            }

            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == updatedTransaction.UserId);
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedTransaction.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == updatedTransaction.AccountId && a.UserId == updatedTransaction.UserId);
            if (account == null)
            {
                return NotFound("Account not found.");
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == updatedTransaction.CategoryId && c.UserId == updatedTransaction.UserId);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            // Update the transaction fields
            transaction.Title = updatedTransaction.Title;
            transaction.Payee = updatedTransaction.Payee;
            transaction.Amount = updatedTransaction.Amount;
            transaction.AccountId = updatedTransaction.AccountId;
            transaction.CategoryId = updatedTransaction.CategoryId;
            transaction.UserId = updatedTransaction.UserId;
            transaction.CreatedAt = updatedTransaction.CreatedAt;

            try
            {
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
                return Ok(transaction); // Return updated transaction
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the transaction.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id, int userId)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            try
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return Ok("Transaction deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the transaction.");
            }
        }


    }
}

using Expenses.Api.Dtos;
using Expenses.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expenses.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController(ITransactionService transactionService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (!TryGetUserId(out var userId))
                return BadRequest("Invalid or missing user ID");

            var transactions = await transactionService.GetAll(userId);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!TryGetUserId(out var userId))
                return BadRequest("Invalid or missing user ID");

            var transaction = await transactionService.Get(id,userId);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateTransactionDto payload)
        {
            if (!TryGetUserId(out var userId))
                return BadRequest("Invalid or missing user ID");

            var transaction = await transactionService.Create(payload, userId);
            return Ok(transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,PutTransactionDto payload)
        {
            if (!TryGetUserId(out var userId))
                return BadRequest("Invalid or missing user ID");

            var transaction = await transactionService.Update(id, payload,userId);
            if(transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!TryGetUserId(out var userId))
                return BadRequest("Invalid or missing user ID");

            var transaction = await transactionService.Get(id, userId);
            if(transaction == null)
            {

                return NotFound();
            }
            await transactionService.Delete(id, userId);
            return Ok();
        }

        #region Private Methods
        private bool TryGetUserId(out int userId)
        {
            userId = 0;

            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claim == null)
                return false;

            return int.TryParse(claim, out userId);
        }
        #endregion
    }
}

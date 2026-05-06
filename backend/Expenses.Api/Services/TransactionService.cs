using Expenses.Api.Data;
using Expenses.Api.Dtos;
using Expenses.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Api.Services
{
    public class TransactionService(ApplicationDbContext context) : ITransactionService
    {
        public async Task<Transaction> Create(CreateTransactionDto transaction, int userId)
        {
            var newTransaction = new Transaction();

            newTransaction.Amount = transaction.Amount;
            newTransaction.Category = transaction.Category;
            newTransaction.Type = transaction.Type;            
            newTransaction.CreatedAt = DateTime.UtcNow;
            newTransaction.UpdatedAt = DateTime.UtcNow;
            newTransaction.UserId = userId;

            await context.Transactions.AddAsync(newTransaction);
            await context.SaveChangesAsync();
            return newTransaction;

        }

        public async Task Delete(int id, int userId)
        {
            Transaction? transaction = await FindTransaction(id, userId);
            if (transaction != null)
            {
                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();
            }
        }     

        public async Task<Transaction?> Get(int id, int userId)
        {
            var transaction = await FindTransaction(id, userId);
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAll(int userId)
        {
            return await context.Transactions.Where(x=>x.UserId == userId).ToListAsync();
        }

        public async Task<Transaction?> Update(int id, PutTransactionDto transaction, int userId)
        {
            var existingTransaction = await FindTransaction(id,userId);
            if (existingTransaction != null)
            {
                existingTransaction.Type = transaction.Type;
                existingTransaction.Category = transaction.Category;
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
            return existingTransaction;
        }

        private async Task<Transaction?> FindTransaction(int id, int userId)
        {
            return await context.Transactions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }
    }
}

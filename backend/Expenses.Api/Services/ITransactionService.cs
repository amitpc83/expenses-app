using Expenses.Api.Dtos;
using Expenses.Api.Models;

namespace Expenses.Api.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAll(int userId);
        Task<Transaction?> Get(int id,int userId);
        Task<Transaction?> Update(int id,PutTransactionDto transaction,int userId);
        Task Delete(int id, int userId);
        Task<Transaction> Create(CreateTransactionDto transaction, int userId);
    }
}

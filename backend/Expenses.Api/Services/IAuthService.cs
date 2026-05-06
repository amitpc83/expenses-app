using Expenses.Api.Dtos;
using Expenses.Api.Models;

namespace Expenses.Api.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        Task<User?> LoginAsync(LoginUserDto loginUserDto);
        Task<User> RegisterAsync(RegisterUserDto registerUserDto);
        Task<bool> UserExistsAsync(string email);
    }
}

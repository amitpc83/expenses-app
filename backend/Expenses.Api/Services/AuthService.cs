using Expenses.Api.Data;
using Expenses.Api.Dtos;
using Expenses.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Expenses.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _hasher = new();
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email.Trim().ToLower());
        }

        public async Task<User> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var email = registerUserDto.Email.Trim().ToLower();
            if (await UserExistsAsync(email))
            {
                throw new Exception("User already exists");
            }
            var user = new User
            {
                CreatedAt = DateTime.UtcNow,
                Email = email,
                UpdatedAt = DateTime.UtcNow
            };
            user.Password = _hasher.HashPassword(user, registerUserDto.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                                        x => x.Email == loginUserDto.Email.Trim().ToLower());
            if (user == null)
            {
                return null;
            }
            var result = _hasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var jwt = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                 issuer : jwt["Issuer"],
                 audience : jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

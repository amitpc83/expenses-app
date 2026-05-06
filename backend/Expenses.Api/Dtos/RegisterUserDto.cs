using System.ComponentModel.DataAnnotations;

namespace Expenses.Api.Dtos
{
    public class RegisterUserDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}

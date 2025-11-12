using System.ComponentModel.DataAnnotations;

namespace EShop.Dto
{
    public class LoginRequestDto(string Email, string Password)
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; init; } = Email;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; init; } = Password;
    }
}

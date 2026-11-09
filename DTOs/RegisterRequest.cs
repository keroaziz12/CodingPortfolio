using System.ComponentModel.DataAnnotations;

namespace CodingPortfolio.DTOs
{
    public class RegisterRequest
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required ,MinLength(8)]
        public string Password { get; set; } = null!;
    }
}

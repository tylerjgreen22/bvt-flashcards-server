using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // DTO for registering users, provides validation to meet Identity criteria, such as complex password
    public class ChangeUserDto
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
        public string NewPassword { get; set; }
        public string Username { get; set; }
    }
}
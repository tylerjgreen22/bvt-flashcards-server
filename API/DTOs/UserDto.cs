using Domain.Entities;

namespace API.DTOs
{
    // DTO for returning user information to client
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string Image { get; set; }
        public ICollection<Picture> Pictures { get; set; }
    }
}
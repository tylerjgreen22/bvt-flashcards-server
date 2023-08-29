using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public ICollection<Set> Sets { get; set; }
        public ICollection<Picture> Pictures { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    // App User, extends from Identity User and contains the users sets and pictures collections
    public class AppUser : IdentityUser
    {
        public ICollection<Set> Sets { get; set; }
        public ICollection<Picture> Pictures { get; set; }
    }
}
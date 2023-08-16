using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    // The data context, registered with the dependency injection container to be injected throughout project and used to interact with the database
    // Extends IdentityDbContext to allow for interaction with Identity in DB
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<Set> Sets { get; set; }
    }
}
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

        // Db sets allow for access to respective tables
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        // Override of the OnModelCreating method from the base DB context class
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the foreign key relationship between sets and flashcards and specify on cascade delete behavior
            builder.Entity<Set>()
                .HasMany(s => s.Flashcards)
                .WithOne(f => f.Set)
                .HasForeignKey(f => f.SetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the foreign key relationship between app users and sets and specify on cascade delete behavior
            builder.Entity<AppUser>()
                .HasMany(s => s.Sets)
                .WithOne(f => f.AppUser)
                .HasForeignKey(f => f.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
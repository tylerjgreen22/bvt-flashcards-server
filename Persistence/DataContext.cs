using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    // The data context, registered with the dependency injection container to be injected throughout project and used to interact with the database
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Flashcard> Flashcards { get; set; }
    }
}
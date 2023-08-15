using System.Text.Json;
using Domain;
using Domain.Entities;

namespace Persistence
{
    // Seed class to seed the database using json files
    public class Seed
    {
        public static async Task SeedAsync(DataContext context)
        {
            // Seeding sets
            if (!context.Sets.Any())
            {
                var setsData = File.ReadAllText("../Persistence/SeedData/sets.json");
                var sets = JsonSerializer.Deserialize<List<Set>>(setsData);
                context.Sets.AddRange(sets);
            }

            // Seeding flashcards
            if (!context.Flashcards.Any())
            {
                var flashcardsData = File.ReadAllText("../Persistence/SeedData/flashcards.json");
                var flashcards = JsonSerializer.Deserialize<List<Flashcard>>(flashcardsData);
                context.Flashcards.AddRange(flashcards);
            }

            // Saving changes if changes were made
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
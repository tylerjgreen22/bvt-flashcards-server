using System.Text.Json;
using Domain;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    // Seed class to seed the database using json files
    public class Seed
    {
        public static async Task SeedAsync(DataContext context, UserManager<AppUser> userManager)
        {
            // Seed a user
            if (!userManager.Users.Any())
            {
                var user = new AppUser { DisplayName = "Quizlit", UserName = "quizlit", Email = "quizlit@test.com" };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

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
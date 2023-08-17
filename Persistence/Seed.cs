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
                var usersData = File.ReadAllText("../Persistence/SeedData/users.json");
                var users = JsonSerializer.Deserialize<List<AppUser>>(usersData);
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
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
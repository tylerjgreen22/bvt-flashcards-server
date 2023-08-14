using Domain;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedAsync(DataContext context)
        {
            if (context.Flashcards.Any()) return;

            var flashcards = new List<Flashcard>
            {
                new Flashcard
                {
                    Term = "What does HTML stand for?",
                    Definition = "Hyper Text Markup Language",
                    PictureUrl = "test",
                },
                new Flashcard
                {
                    Term = "What does CSS stand for?",
                    Definition = "An HTML container",
                    PictureUrl = "test",
                },
                new Flashcard
                {
                    Term = "What is the correct HTML element for the largest heading?",
                    Definition = "<h1>",
                    PictureUrl = "test",
                },
                new Flashcard
                {
                    Term = "Which HTML attribute specifies an alternate text for an image, if the image cannot be displayed?",
                    Definition = "alt",
                    PictureUrl = "test",
                },
                new Flashcard
                {
                    Term = "Which CSS property is used to change the text color of an element?",
                    Definition = "color",
                    PictureUrl = "test",
                },
                new Flashcard
                {
                    Term = "What is JavaScript?",
                    Definition = "JavaScript is a programming language",
                    PictureUrl = "test",
                },
                new Flashcard
                {
                    Term = "What is the output of the following code?\n\nconsole.log(2 + '2');",
                    Definition = "22",
                    PictureUrl = "test",
                },
                new Flashcard
                {
                    Term = "What is the correct way to create a JavaScript array?",
                    Definition = "var colors = [\"red\", \"green\", \"blue\"]",
                    PictureUrl = "test",
                },
                new Flashcard
                {
                    Term = "Which built-in method removes the last element from an array and returns that element?",
                    Definition = "pop()",
                    PictureUrl = "test",
                },
                new Flashcard
                {
                    Term = "Which operator is used to assign a value to a variable?",
                    Definition = "=",
                    PictureUrl = "test",
                },
            };

            await context.Flashcards.AddRangeAsync(flashcards);
            await context.SaveChangesAsync();
        }
    }
}
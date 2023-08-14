using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    // Flashcard controller for interacting with the flashcards
    public class FlashcardsController : BaseApiController
    {
        // Field for the data context
        private readonly DataContext _context;

        // Injecting data context to interact with database
        public FlashcardsController(DataContext context)
        {
            _context = context;
        }

        // All methods return non-blocking tasks

        // Get method to get all flashcards
        [HttpGet]
        public async Task<ActionResult<List<Flashcard>>> GetFlashcards()
        {
            return await _context.Flashcards.ToListAsync();
        }
    }
}
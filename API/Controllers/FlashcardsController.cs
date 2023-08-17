using Application.Flashcards;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Flashcard controller for interacting with the flashcards
    public class FlashcardsController : BaseApiController
    {
        // All methods return non-blocking tasks and utilize Mediator to perform operations. The return values are wrapped in the HandleResult method to handle errors

        // Get method to get all flashcards by the set Id
        [HttpGet("{setId}")]
        public async Task<ActionResult<List<Flashcard>>> GetFlashcards(Guid setId)
        {
            return HandleResult(await Mediator.Send(new ListFlashcards.Query { SetId = setId }));
        }

        // Post method to create flashcards
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFlashcards(Flashcard[] flashcards)
        {
            return HandleResult(await Mediator.Send(new CreateFlashcards.Command { Flashcards = flashcards }));
        }

        // Put method to update flashcards based on set ID
        [Authorize]
        [HttpPut("{setId}")]
        public async Task<IActionResult> EditFlashcards(Guid setId, [FromBody] Flashcard[] flashcards)
        {
            return HandleResult(await Mediator.Send(new EditFlashcards.Command { SetId = setId, Flashcards = flashcards }));
        }
    }
}
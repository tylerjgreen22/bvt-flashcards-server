using Application.Flashcards;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Flashcard controller for interacting with the flashcards
    public class FlashcardsController : BaseApiController
    {
        // All methods return non-blocking tasks and utilize Mediator to perform operations. The return values are wrapped in the HandleResult method to handle errors

        // Get method to get all flashcards
        [HttpGet]
        public async Task<ActionResult<List<Flashcard>>> GetFlashcards()
        {
            return HandleResult(await Mediator.Send(new ListFlashcards.Query()));
        }

        // Get method to get a single flashcard based on ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Flashcard>> GetFlashcard(int id)
        {
            return HandleResult(await Mediator.Send(new DetailedFlashcard.Query { Id = id }));
        }

        // Post method to create a flashcard
        [HttpPost]
        public async Task<IActionResult> CreateFlashcard(Flashcard flashcard)
        {
            return HandleResult(await Mediator.Send(new CreateFlashcard.Command { Flashcard = flashcard }));
        }

        // Put method to update a flashcard based on ID
        [HttpPut("{id}")]
        public async Task<IActionResult> EditFlashcard(int id, Flashcard flashcard)
        {
            flashcard.Id = id;
            return HandleResult(await Mediator.Send(new EditFlashcard.Command { Flashcard = flashcard }));
        }

        // Delete method to delete a flashcard based on ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlashcard(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteFlashcard.Command { Id = id }));
        }
    }
}
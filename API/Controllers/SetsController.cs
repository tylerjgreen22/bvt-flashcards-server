using Application.Sets;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Sets controller for interacting with sets
    public class SetsController : BaseApiController
    {
        // All methods return non-blocking tasks and utilize Mediator to perform operations

        // Get method to get all sets
        [HttpGet]
        public async Task<ActionResult<List<Set>>> GetSets()
        {
            return HandleResult(await Mediator.Send(new ListSets.Query()));
        }

        // Get method to get a single set by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Set>> GetSet(int id)
        {
            return HandleResult(await Mediator.Send(new DetailedSet.Query { Id = id }));
        }

        // Post method to create a set
        [HttpPost]
        public async Task<IActionResult> CreateSet(Set set)
        {
            return HandleResult(await Mediator.Send(new CreateSet.Command { Set = set }));
        }

        // Put method to update a set based on ID
        [HttpPut("{id}")]
        public async Task<IActionResult> EditSet(int id, Set set)
        {
            set.Id = id;
            return HandleResult(await Mediator.Send(new EditSet.Command { Set = set }));
        }

        // Delete method to delete a set based on ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSet(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteSet.Command { Id = id }));
        }
    }
}
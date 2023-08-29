using Application.Core;
using Application.Sets;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Sets controller for interacting with sets
    public class SetsController : BaseApiController
    {
        // All methods return non-blocking tasks and utilize Mediator to perform operations

        // Get method to get all sets
        [HttpGet]
        public async Task<ActionResult<List<Set>>> GetSets([FromQuery] PagingParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListSets.Query { Params = param }));
        }

        // Get method to get a single set by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Set>> GetSet(string id)
        {
            return HandleResult(await Mediator.Send(new DetailedSet.Query { Id = id }));
        }

        // Post method to create a set
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSet(Set set)
        {
            return HandleResult(await Mediator.Send(new CreateSet.Command { Set = set }));
        }

        // Put method to update a set based on ID
        [Authorize(Policy = "IsOwner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditSet(string id, Set set)
        {
            set.Id = id;
            return HandleResult(await Mediator.Send(new EditSet.Command { Set = set }));
        }

        // Delete method to delete a set based on ID
        [Authorize(Policy = "IsOwner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSet(string id)
        {
            return HandleResult(await Mediator.Send(new DeleteSet.Command { Id = id }));
        }
    }
}
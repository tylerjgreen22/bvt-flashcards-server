using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Base API controller used to extend subsequent API controllers. Gets access to the Mediator service
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        // Obtaining mediator from services if it doesnt already exist
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
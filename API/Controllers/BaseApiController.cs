using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Base API controller used to extend subsequent API controllers
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}
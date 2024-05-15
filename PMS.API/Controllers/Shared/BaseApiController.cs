using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PMS.API.Controllers.Shared;

[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("/api/v{v:apiVersion}/[controller]")]
public class BaseApiController : ControllerBase
{
}

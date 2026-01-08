using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController:ControllerBase
    {
        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            return Ok("you are authorized and logged in!");
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult Public()
        {
            return Ok("This is a public end point");
        }
    }
}

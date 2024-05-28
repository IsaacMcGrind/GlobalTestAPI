using Microsoft.AspNetCore.Mvc;

namespace GlobalTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("throw")]
        public IActionResult Throw()
        {
            throw new Exception("Test exception");
        }

        [HttpGet]
        [Route("throw-arg")]
        public IActionResult ThrowArgument()
        {
            throw new ArgumentException("Test argument exception");
        }

        [HttpGet]
        [Route("throw-auth")]
        public IActionResult ThrowUnauthorized()
        {
            throw new UnauthorizedAccessException("Test unauthorized exception");
        }
    }
}

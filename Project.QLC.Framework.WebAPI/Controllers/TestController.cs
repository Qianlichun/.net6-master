using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.QLC.Framework.WebAPI.Extension;

namespace Project.QLC.Framework.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : BaseController
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var aaa = userInfo;
            _logger.LogInformation("1234");
            _logger.LogError("12345");
            _logger.LogWarning("123456");
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetDate()
        {
            return new JsonResult(new { Date = DateTime.UtcNow });
        }
    }
}

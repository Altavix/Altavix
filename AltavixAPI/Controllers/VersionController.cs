using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AltavixAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";
            
            return Ok(new
            {
                Version = version,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {

        }

        [HttpGet("2")]
        [Authorize("Permissions.Geral.ReadAdmin")]
        public IActionResult Get2()
        {
            return Ok(new { OK = "OK" });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Broker")]
        public IActionResult Get()
        {
            return Ok(new { OK = "OK" });
        }
    }
}
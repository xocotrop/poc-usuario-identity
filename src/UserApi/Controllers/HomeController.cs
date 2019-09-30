using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UserApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {

        }

        [HttpGet("ListPolicies")]
        [Authorize("ViewPolicies")]
        public IActionResult ListPolicies()
        {
            var list = new List<string>
            {
                "1",
                "2",
                "3"
            };
            return Ok(list);
        }

        [HttpGet("2")]
        [Authorize(Policy = "Broker")]
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
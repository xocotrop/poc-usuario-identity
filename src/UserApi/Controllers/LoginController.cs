using Microsoft.AspNetCore.Mvc;
using UserApi.Security;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public readonly AccessManager _accessManager;
        public LoginController(AccessManager accessManager)
        {
            _accessManager = accessManager;
        }

        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (_accessManager.ValidateCredentials(user))
            {
                return Ok(_accessManager.GenerateToken(user));
            }
            return BadRequest(new { Error = "Erro ao gerar o token" });
        }
    }
}
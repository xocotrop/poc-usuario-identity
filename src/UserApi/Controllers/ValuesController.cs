using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserData;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        
        public ValuesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            //var cRole = await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            //var user = new ApplicationUser { UserName = "igor_cor", Email = "igor2.sms@gmail.com", Name = "Igor Janoski dos Santos", EmailConfirmed = true };
            //var create = await _userManager.CreateAsync(user, "Igor2076!");

            //if (!create.Succeeded)
            //{
            //    var u = await _userManager.FindByNameAsync("igor_cor");
            //    if(!await _userManager.CheckPasswordAsync(u, "Igor2076!"))
            //    {
            //        //_userManager.SetLockoutEnabledAsync();
            //        var t3 = await _userManager.AccessFailedAsync(u);
            //        if(t3.Succeeded){
            //            Console.WriteLine("Sucesso");
            //        }
            //    }
            //    var r = await _userManager.AddToRoleAsync(u, "Admin");

            //    var role = await _roleManager.FindByNameAsync("Admin");
            //    var c = new Claim(CustomClaimTypes.Permission, "Read");
            //    var t = await _roleManager.AddClaimAsync(role, c);

            //    var uclaims = await _userManager.GetClaimsAsync(u);
                
            //    if(!uclaims.Any(aa => aa.Type== CustomClaimTypes.Permission && aa.Value ==  "Read"))
            //    {
            //    var r2 = await _userManager.AddClaimAsync(u, c);
            //                        Console.WriteLine("Já existe a claim");
            //    }


            //    return BadRequest(create.Errors);
            //}

            //var claims = new List<Claim>
            //{
            //    new Claim(JwtClaimTypes.Role, "Policyholder")
            //};


            //await _userManager.AddClaimsAsync(user, claims);

            //_userManager.GenerateEmailConfirmationTokenAsync()

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public static class CustomClaimTypes
    {
        public readonly static string Permission = "Permission";
    }
}

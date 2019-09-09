using Microsoft.AspNetCore.Identity;
using System;

namespace UserData
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
    }
}

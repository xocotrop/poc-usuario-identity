using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace UserData.AppContext
{
    public class UserDataContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
        {

        }
    }
}

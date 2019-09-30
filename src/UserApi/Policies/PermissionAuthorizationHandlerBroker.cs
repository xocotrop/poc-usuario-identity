using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using UserData;

namespace UserApi.Policies
{
    internal class PermissionAuthorizationHandlerBroker : AuthorizationHandler<PermissionRequirementBroker>
    {
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole<Guid>> _roleManager;

        public PermissionAuthorizationHandlerBroker(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override Task HandleAsync(AuthorizationHandlerContext context)
        {

            return base.HandleAsync(context);
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirementBroker requirement)
        {

            if (!context.PendingRequirements.Any())
                return;

            if (context.User == null)
            {
                context.Fail();
                return;
            }

            if (context.PendingRequirements.Any(p => p is PermissionRequirementBroker) && context.User.IsInRole("Broker"))
            {
                var p = context.PendingRequirements.Where(pr => pr is PermissionRequirementBroker).Cast<PermissionRequirementBroker>().FirstOrDefault(pr => pr.Role == "broker");
                if (string.IsNullOrWhiteSpace(p.Permission))
                {
                    foreach (var item in context.Requirements)
                    {
                        context.Succeed(item);
                    }
                }
                if (!context.PendingRequirements.Any())
                {
                    return;
                }
            }


            var aa = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            foreach (var a in context.PendingRequirements.ToList())
            {
                context.Succeed(a);
            }

            // Get all the roles the user belongs to and check if any of the roles has the permission required
            // for the authorization to succeed.
            var user = await _userManager.GetUserAsync(context.User);
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name));
            var userClaims = await _userManager.GetClaimsAsync(user);

            var permission = userClaims.Where(x => x.Type == "Permissions" && x.Value == requirement.Permission).Select(x => x.Value);

            //if (permission.Any())
            //{
            //    context.Succeed(requirement);
            //    return;
            //}

            ////método de claims nas roles
            //foreach (var role in userRoles)
            //{
            //    var roleClaims = await _roleManager.GetClaimsAsync(role);
            //    var permissions = roleClaims.Where(x => x.Type == "Permission" &&
            //                                            x.Value == requirement.Permission &&
            //                                            x.Issuer == "LOCAL AUTHORITY")
            //                                .Select(x => x.Value);

            //    if (permissions.Any())
            //    {
            //        context.Succeed(requirement);
            //        return;
            //    }
            //}
            //return;
            //context.Fail();
        }
    }
}
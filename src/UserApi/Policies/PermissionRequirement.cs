using Microsoft.AspNetCore.Authorization;

namespace UserApi.Policies
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public string Role { get; private set; }

        public void SetPermission(string permission)
        {
            Permission = permission;
        }

        public PermissionRequirement(string role)
        {
            Role = role;
        }

        public PermissionRequirement(string permission, string role)
        {
            Role = role;
            Permission = permission;
        }
    }
}
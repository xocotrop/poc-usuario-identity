using Microsoft.AspNetCore.Authorization;

namespace UserApi.Policies
{
    internal class PermissionRequirementBroker : IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public string Role { get; private set; }

        public void SetPermission(string permission)
        {
            Permission = permission;
        }

        public PermissionRequirementBroker(string role)
        {
            Role = role;
        }

        public PermissionRequirementBroker(string permission, string role)
        {
            Role = role;
            Permission = permission;
        }
    }
}
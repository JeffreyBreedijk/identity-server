using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Utilize.Identity.Manager.Authorization
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission{ get; private set; }
    }
    
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "permissions"))
            {
                return Task.CompletedTask;
            }

            var p = JsonConvert.DeserializeObject<List<string>>(context.User.FindFirst(c => c.Type.Equals("permissions")).Value);

            if (p.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }


            return Task.CompletedTask;
        }
    }
}
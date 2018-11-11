using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Utilize.Identity.Provider.Models;

namespace Utilize.Identity.Provider.Services
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims =  new List<Claim>()
            {
                new Claim("user_id", "tester")

                
                
              
            };
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);

        }
        
       
    }
}
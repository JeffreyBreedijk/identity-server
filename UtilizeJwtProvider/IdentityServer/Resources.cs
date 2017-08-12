using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace UtilizeJwtProvider.IdentityServer
{
    internal class Resources
    {        
        public static IEnumerable<ApiResource> GetApiResources() {
            return new List<ApiResource> {
                new ApiResource {
                    Name = "UtilizeAPI",
                    DisplayName = "Utilize API",
                    Description = "All Utilize supplied API's",
                    UserClaims = new List<string> {JwtClaimTypes.Name, JwtClaimTypes.Role, "role"},
//                    ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
//                    Scopes = new List<Scope> {
//                        new Scope("customAPI.read"),
//                        new Scope("customAPI.write")
//                    }
                }
            };
        }
    }
}
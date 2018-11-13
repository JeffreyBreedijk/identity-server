using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace Utilize.Identity.Provider
{
    public class Config
    {
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            var r = new ApiResource("api1", "My API") {ApiSecrets = new List<Secret>() {new Secret("secret".Sha256())}};
            return new List<ApiResource>
            {
                r
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AccessTokenType = AccessTokenType.Reference,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                }
            };
        }
    }
}
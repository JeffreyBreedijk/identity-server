using System;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace UtilizeJwtProvider.IdentityServer
{
    public class Config
    {
        
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "Utilize API",
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "utilize.default",
                            DisplayName = "Utilize Default Scope",
                            UserClaims =
                            {
                                JwtClaimTypes.Name, 
                                JwtClaimTypes.Email,
                                JwtClaimTypes.FamilyName,
                                JwtClaimTypes.GivenName,
                                JwtClaimTypes.Role,
                                "debtor_id"
                            }
                        }
                      
                    }
                }
            };
        }
        
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "utilize",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "utilize.default"}
                }
                
             
            };
        }

    }
}
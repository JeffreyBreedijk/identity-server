using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using IdentityServer4.Models;

namespace Utilize.Identity.Provider.Models
{
    public class SimpleClient
    {
        public string ClientId { get; set; }
        public string Description { get; set; }
        public string ClientName { get; set; }
        public AccessTokenType AccessTokenType { get; set; }
        public int AccessTokenLifetime { get; set; } = 3600;
        public List<string> AllowedScopes { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
    }

    public static class SimpleClientExtensions
    {
        public static Client ToIdentityServer4Client(this SimpleClient c)
        {
            return new Client
            {
                Description = c.Description,
                ClientName = c.ClientName,
                AllowedScopes = c.AllowedScopes,
                AllowedGrantTypes = c.AllowedGrantTypes,
                AccessTokenType = c.AccessTokenType,
                AccessTokenLifetime = c.AccessTokenLifetime
            };
        }
    }

    public class SimpleClientValidator : AbstractValidator<SimpleClient>
    {
        private static readonly List<string> ValidGrantTypes = new List<string>
        {
            GrantType.Hybrid, GrantType.Implicit, GrantType.AuthorizationCode, GrantType.AuthorizationCode,
            GrantType.ClientCredentials, GrantType.DeviceFlow, GrantType.ResourceOwnerPassword
        };

        public SimpleClientValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
                RuleFor(x => x.ClientName).NotEmpty().WithMessage("Client name cannot be empty");
                RuleFor(x => x.AllowedScopes).NotEmpty().WithMessage("There must be at least 1 scope defined");
                RuleFor(x => x.AllowedGrantTypes).NotEmpty().WithMessage("There must be at least 1 grant type defined");
                RuleFor(x => x.AllowedGrantTypes).Must(IsValidGrantType).WithMessage("Invalid grant type specified");
            });
        }

        private static bool IsValidGrantType(List<string> grantTypes)
        {
            return grantTypes.All(x => ValidGrantTypes.Contains(x.ToLowerInvariant()));
        }
    }
}
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using UtilizeJwtProvider.Domain.Aggregates;
using UtilizeJwtProvider.Repository;
using UtilizeJwtProvider.Services;

namespace UtilizeJwtProvider.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = _userRepository.FindUserByEmail(context.UserName);
                if (user != null)
                {
                    if (_passwordService.ValidatePassword(context.Password, user.Salt, user.Hash))
                    {
                        context.Result = new GrantValidationResult(
                            subject: user.Email.ToString(),
                            authenticationMethod: "custom",
                            claims: GetUserClaims(user));

                        return;
                    }
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
                return;
            }
            catch (Exception ex)
            {
                context.Result =
                    new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }

        public static Claim[] GetUserClaims(User user)
        {
            return new[]
            {
                new Claim("user_id", user.Id.ToString() ?? ""),
                new Claim(type: JwtClaimTypes.Name,
                    value: (!string.IsNullOrEmpty(user.Firstname) && !string.IsNullOrEmpty(user.Lastname))
                        ? (user.Firstname + " " + user.Lastname)
                        : ""),
                new Claim(JwtClaimTypes.GivenName, user.Firstname ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.Lastname ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? ""),
                new Claim("debtor_id", user.DebtorId ?? ""),

//                roles
//                new Claim(JwtClaimTypes.Role, user.Role)
            };
        }
    }
}
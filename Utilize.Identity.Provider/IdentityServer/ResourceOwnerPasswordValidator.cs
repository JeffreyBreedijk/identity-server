using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Newtonsoft.Json;
using Utilize.Identity.Provider.Models;
using IPasswordService = Utilize.Identity.Provider.Services.IPasswordService;
using IUserService = Utilize.Identity.Provider.Services.IUserService;

namespace Utilize.Identity.Provider.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;

        public ResourceOwnerPasswordValidator(IUserService userService, IPasswordService passwordService)
        {
            _userService = userService;
            _passwordService = passwordService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = _userService.GetUser(context.Request.Client.ClientId, context.UserName).Result;
                if (user != null)
                {
                    if (_passwordService.ValidatePassword(context.Password, user.Salt, user.Hash))
                    {
                        context.Result = new GrantValidationResult(
                            subject: user.LoginCode.ToString(),
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
          
            var claims = new List<Claim>()
            {
                new Claim("user_id", user.LoginCode.ToString() ?? ""),
                new Claim(type: JwtClaimTypes.Name,
                    value: (!string.IsNullOrEmpty(user.Firstname) && !string.IsNullOrEmpty(user.Lastname))
                        ? (user.Firstname + " " + user.Lastname)
                        : ""),
                new Claim(JwtClaimTypes.GivenName, user.Firstname ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.Lastname ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? ""),
                new Claim("debtor_id", user.DebtorId ?? ""),
                new Claim("permissions", JsonConvert.SerializeObject(new List<string>() {"utilize-admin"})),
                new Claim("licences", JsonConvert.SerializeObject(new List<string>() {"testlicence"}))

                
                
              
            };
            
            //user.Roles.ToList().ForEach(r => claims.Add(new Claim(JwtClaimTypes.Role, r)));

            return claims.ToArray();
        }
    }
}
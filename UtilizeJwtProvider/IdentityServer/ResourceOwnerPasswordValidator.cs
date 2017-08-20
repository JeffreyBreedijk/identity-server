//using System;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using IdentityModel;
//using IdentityServer4.Models;
//using IdentityServer4.Validation;
//using UtilizeJwtProvider.Domain.Aggregates;
//using UtilizeJwtProvider.Models;
//using UtilizeJwtProvider.Repository;
//
//namespace UtilizeJwtProvider.Services
//{
//    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
//    {
////        private readonly IUserRepository _userRepository;
//        private readonly IPasswordService _passwordService;
//
//        public ResourceOwnerPasswordValidator(IUserRepository userRepository, IPasswordService passwordService)
//        {
//            _userRepository = userRepository;
//            _passwordService = passwordService;
//        }
//
//        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
//        {
//            try
//            {
//                //get your user model from db (by username - in my case its email)
//                var user = await _userRepository.FindAsync(context.UserName);
//                if (user != null)
//                {
//                    //check if password match - remember to hash password if stored as hash in db
//                    if (_passwordService.VerifyPassword(user.Hash, user.Salt, context.Password))
//                    {
//                        //set the result
//                        context.Result = new GrantValidationResult(
//                            subject: user.UserId.ToString(),
//                            authenticationMethod: "custom",
//                            claims: GetUserClaims(user));
//
//                        return;
//                    }
//                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
//                    return;
//                }
//                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
//                return;
//            }
//            catch (Exception ex)
//            {
//                context.Result =
//                    new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
//            }
//        }
//
//        public static Claim[] GetUserClaims(User user)
//        {
//            return new Claim[]
//            {
////                new Claim("user_id", user.UserId.ToString() ?? ""),
////                new Claim(type: JwtClaimTypes.Name,
////                    value: (!string.IsNullOrEmpty(user.Firstname) && !string.IsNullOrEmpty(user.Lastname))
////                        ? (user.Firstname + " " + user.Lastname)
////                        : ""),
////                new Claim(JwtClaimTypes.GivenName, user.Firstname ?? ""),
////                new Claim(JwtClaimTypes.FamilyName, user.Lastname ?? ""),
////                new Claim(JwtClaimTypes.Email, user.Email ?? ""),
////                new Claim("debtor_id", user.DebtorId ?? ""),
//
//                //roles
//                //new Claim(JwtClaimTypes.Role, user.Role)
//            };
//        }
//    }
//}
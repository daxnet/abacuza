using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Abacuza.Services.Identity.Models
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        //repository to get user from db
        private readonly UserManager<AbacuzaAppUser> _userRepository;
        private readonly RoleManager<AbacuzaAppRole> _roleRepository;

        public ResourceOwnerPasswordValidator(UserManager<AbacuzaAppUser> userRepository, RoleManager<AbacuzaAppRole> roleRepository)
        {
            _userRepository = userRepository; //DI
            _roleRepository = roleRepository;
        }

        //this is used to validate your user account with provided grant at /connect/token
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                //get your user model from db (by username - in my case its email)
                var user = await _userRepository.FindByNameAsync(context.UserName);
                if (user != null)
                {
                    //check if password match - remember to hash password if stored as hash in db
                    if (await _userRepository.CheckPasswordAsync(user, context.Password))
                    {
                        //set the result
                        context.Result = new GrantValidationResult(
                            subject: user.UserName,
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
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }

        //build claims array from user data
        public static Claim[] GetUserClaims(AbacuzaAppUser user)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Email, user.Email  ?? ""),
                new Claim(JwtClaimTypes.Name, user.DisplayName  ?? ""),
                new Claim(JwtClaimTypes.Role, "admin") // TODO
            };
        }
    }
}

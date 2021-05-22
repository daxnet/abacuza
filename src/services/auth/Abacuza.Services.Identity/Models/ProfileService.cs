using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Services.Identity.Models
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AbacuzaAppUser> _userRepository;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(UserManager<AbacuzaAppUser> userRepository, ILogger<ProfileService> logger)
            => (_userRepository, _logger) = (userRepository, logger);

        //Get user profile date in terms of claims when calling /connect/userinfo
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    //get user from db (in my case this is by email)
                    var user = await _userRepository.FindByNameAsync(context.Subject.Identity.Name);

                    if (user != null)
                    {
                        var roles = await _userRepository.GetRolesAsync(user);
                        var claims = ResourceOwnerPasswordValidator.GetUserClaims(user, roles);

                        //set issued claims to return
                        context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                    }
                }
                else
                {
                    //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    //where and subject was set to my user id.
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    if (!string.IsNullOrEmpty(userId?.Value))
                    {
                        //get user from db (find user by user id)
                        var user = await _userRepository.FindByNameAsync(userId.Value);
                        var roles = await _userRepository.GetRolesAsync(user);

                        // issue the claims for the user
                        if (user != null)
                        {
                            var claims = ResourceOwnerPasswordValidator.GetUserClaims(user, roles);

                            context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read user profile.");
            }
        }

        //check if user account is active.
        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                //get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                if (!string.IsNullOrEmpty(userId?.Value))
                {
                    var user = await _userRepository.FindByIdAsync(userId.Value);

                    if (user != null)
                    {
                        context.IsActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to set user active.");
            }
        }
    }
}

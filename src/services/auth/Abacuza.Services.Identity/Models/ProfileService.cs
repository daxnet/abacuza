// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020-2021 by daxnet. All rights reserved.
// Apache License Version 2.0
// ==============================================================

using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Abacuza.Services.Identity.Models
{
    /// <summary>
    /// Represents the service that provides profile information for the logged in user.
    /// </summary>
    public sealed class ProfileService : IProfileService
    {

        #region Private Fields

        private readonly ILogger<ProfileService> _logger;
        private readonly UserManager<AbacuzaAppUser> _userRepository;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>ProfileService</c> class.
        /// </summary>
        /// <param name="userRepository">The user repository that stores the user information.</param>
        /// <param name="logger">The logger instance.</param>
        public ProfileService(UserManager<AbacuzaAppUser> userRepository, ILogger<ProfileService> logger)
            => (_userRepository, _logger) = (userRepository, logger);

        #endregion Public Constructors

        #region Public Methods

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var userId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
                var user = await _userRepository.FindByIdAsync(userId);

                if (user != null)
                {
                    var roles = await _userRepository.GetRolesAsync(user);
                    var claims = GetUserClaims(user, roles);

                    //set issued claims to return
                    context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read user profile.");
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

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

        #endregion Public Methods

        #region Private Methods

        private static Claim[] GetUserClaims(AbacuzaAppUser user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Email, user.Email  ?? ""),
                new Claim(JwtClaimTypes.Name, user.DisplayName  ?? "")
            };

            if (roles != null && roles.Any())
            {
                claims.AddRange(roles.Select(r => new Claim(JwtClaimTypes.Role, r)));
            }

            claims.Add(new Claim(JwtClaimTypes.Role, "regular"));

            return claims.ToArray();
        }

        #endregion Private Methods
    }
}
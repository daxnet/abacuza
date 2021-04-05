
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Abacuza.Services.Identity.Models
{
    public static class IdentityConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Phone()
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new []
            {
                new ApiResource("api.common", "Common Service")
                {
                    Scopes =
                    {
                        "api.common.full_access"
                    },
                    UserClaims =
                    {
                        ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Email, ClaimTypes.Role
                    }
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new []
            {
                new Client
                {
                    RequireConsent = false,
                    ClientId = "web",
                    ClientName = "Abacuza Administrator",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "address", "phone", "api.common.full_access" },
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                }
            };
    }
}

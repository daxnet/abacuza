
using IdentityModel;
using IdentityServer4;
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
                new IdentityResource("roles", "User Roles", new[]{"role"})
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new[]
            {
                new ApiResource("api", "Default API Resource")
                {
                    Scopes =
                    {
                        "api"
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope("api", "Default API Resource", new[]
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Role,
                })
            };

        public static IEnumerable<Client> GetClients() =>
            new[]
            {
                new Client
                {
                    RequireConsent = false,
                    ClientId = "web",
                    ClientName = "Abacuza Administrator",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "api"
                    },
                    RedirectUris = { "http://localhost:4200/auth-callback" },
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200", "http://localhost:9050"},
                    AllowAccessTokensViaBrowser = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 3600,
                }
            };
    }
}

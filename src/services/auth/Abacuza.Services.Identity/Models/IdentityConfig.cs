
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
                new IdentityResource("roles", "User Roles", new[]{ "role" })
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new[]
            {
                new ApiResource("weather.api", "Weather API")
                {
                    Scopes =
                    {
                        "api.weather.full_access"
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope("api.weather.full_access", "Weather API Full Access", new[] {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Role
                    })
            };

        public static IEnumerable<Client> GetClients() =>
            new[]
            {
                new Client
                {
                    RequireConsent = false,
                    ClientId = "mvc",
                    ClientName = "ASP.NET Core MVC",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("mysecret".Sha256())
                    },
                    AllowedScopes = { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "api.weather.full_access" },
                    RedirectUris = {"https://localhost:9800/signin-oidc"},
                    PostLogoutRedirectUris = {"https://localhost:9800/signout-callback-oidc"},
                    AllowedCorsOrigins = {"https://localhost:9800"},
                    AllowAccessTokensViaBrowser = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 3600
                },
                new Client
                {
                    RequireConsent = false,
                    ClientId = "console",
                    ClientName = "Console App for Test",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("mysecret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "api.weather.full_access" },
                    RedirectUris = {"https://localhost:9800/signin-oidc"},
                    PostLogoutRedirectUris = {"https://localhost:9800/signout-callback-oidc"},
                    AllowedCorsOrigins = {"https://localhost:9800"},
                    AllowAccessTokensViaBrowser = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 3600
                }
            };
    }
}

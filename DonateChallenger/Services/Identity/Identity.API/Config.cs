using System;
using System.Collections.Generic;
using System.IO;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Identity.API
{
    public static class Config
    {
        private static IConfiguration _configuration;

        public static IEnumerable<IdentityResource> Resources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> Scopes => new List<ApiScope>
        {
            new ApiScope("react", "React client"),
            new ApiScope("identity.api", "Identity API"),
            new ApiScope("challengeCatalog", "Challenge Catalog"),
        };

        public static IEnumerable<Client> GetClients()
        {
            _configuration = GetConfiguration();
            var reactClientUrl = _configuration?["ReactClientUrl"] ?? throw new ArgumentNullException();
            var challengeCatalogUrl = _configuration?["ChallengeCatalogUrl"] ?? throw new ArgumentNullException();
            var globalUrl = _configuration?["GlobalUrl"] ?? throw new ArgumentNullException();
            return new List<Client>
            {
                new Client
                {
                    ClientId = "spa",
                    ClientName = "Donate-Challenger SPA OpenId Client",
                    ClientUri = globalUrl,

                    AllowedGrantTypes = GrantTypes.Code,

                    RequireConsent = false,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =
                    {
                        $"{globalUrl}/",
                        $"{globalUrl}/signin/callback"
                    },
                    PostLogoutRedirectUris =
                    {
                        $"{globalUrl}/"
                    },
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedCorsOrigins =
                    {
                        globalUrl
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "challengeCatalog",
                        "react",
                        "identity.api"
                    }
                },
                new Client
                {
                    ClientId = "challengecatalogswaggerui",
                    ClientName = "Challenge Catalog",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { $"{challengeCatalogUrl}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{challengeCatalogUrl}/swagger/" },

                    AllowedScopes =
                    {
                        "challengeCatalog"
                    }
                },
            };
        }

        private static IConfiguration GetConfiguration() => Helpers.ConfigurationProvider.GetConfiguration(Directory.GetCurrentDirectory());
    }
}

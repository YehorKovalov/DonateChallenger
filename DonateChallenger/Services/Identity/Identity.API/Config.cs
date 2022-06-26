﻿namespace Identity.API
{
    public static class Config
    {
        private static IConfiguration? _configuration;

        public static IEnumerable<IdentityResource> Resources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiResource> APIs => new List<ApiResource>
        {
            new ApiResource("challengeCatalog", "Challenge Catalog API"),
        };

        public static IEnumerable<Client> GetClients()
        {
            _configuration = GetConfiguration();
            var reactClientUrl = _configuration?["ReactClientUrl"] ?? throw new ArgumentNullException();
            var challengeCatalogUrl = _configuration?["ChallengeCatalogUrl"] ?? throw new ArgumentNullException();
            return new List<Client>
            {
                new Client
                {
                    ClientId = "spa",
                    ClientName = "Donate-Challenger SPA OpenId Client",

                    AllowedGrantTypes = GrantTypes.Code,

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris = { $"{reactClientUrl}/" },

                    PostLogoutRedirectUris = { $"{reactClientUrl}/" },
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedCorsOrigins = { reactClientUrl },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "challengeCatalog",
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

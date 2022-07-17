using System.IO;

namespace Identity.API
{
    public static class Config
    {
        private static IConfiguration _configuration;
        private const string RoleClaim = "role";
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = RoleClaim,
                UserClaims = new List<string> { RoleClaim }
            }
        };

        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource
            {
                Name = "challenge-catalog",
                DisplayName = "Challenge Catalog",
                Scopes = new List<string> { "challenge-catalog.bff" },
                ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                UserClaims = new List<string> { RoleClaim }
            },
            new ApiResource
            {
                Name = "challenge-order",
                DisplayName = "Challenge Order",
                Scopes = new List<string> { "challenge-order.bff" },
                ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                UserClaims = new List<string> { RoleClaim }
            },
            new ApiResource
            {
                Name = "payment",
                DisplayName = "Payment",
                Scopes = new List<string> { "paypal-payment.bff", "paypal-payment.execute" },
                ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                UserClaims = new List<string> { RoleClaim }
            },
            new ApiResource
            {
                Name = "challenges-temporary-storage",
                DisplayName = "Challenges Temporary Storage",
                Scopes = new List<string> { "challenge-temporary-storage.bff" },
                ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                UserClaims = new List<string> { RoleClaim }
            },
        };

        public static IEnumerable<ApiScope> Scopes => new List<ApiScope>
        {
            new ApiScope("challenge-catalog.bff", "Challenge Catalog BFF") { UserClaims = { RoleClaim } },
            new ApiScope("challenge-order.bff", "Challenge Order BFF") { UserClaims = { RoleClaim } },
            new ApiScope("paypal-payment.bff", "Paypal Payment BFF") { UserClaims = { RoleClaim} },
            new ApiScope("challenges-temporary-storage.bff", "Challenges Temporary Storage BFF") { UserClaims = { RoleClaim } },
        };

        public static IEnumerable<Client> GetClients()
        {
            _configuration = GetConfiguration();
            var reactClientUrl = _configuration?["ReactClientUrl"] ?? throw new ArgumentNullException();
            var challengeCatalogUrl = _configuration?["ChallengeCatalogUrl"] ?? throw new ArgumentNullException();
            var challengeOrderUrl = _configuration?["ChallengeOrderUrl"] ?? throw new ArgumentNullException();
            var paymentUrl = _configuration?["PaymentUrl"] ?? throw new ArgumentNullException();
            var challengesTemporaryStorageUrl = _configuration?["ChallengesTemporaryStorageUrl"] ?? throw new ArgumentNullException();
            var globalUrl = _configuration?["GlobalUrl"] ?? throw new ArgumentNullException();

            return new List<Client>
            {
                new Client
                {
                    ClientId = "spa",
                    ClientName = "Donate-Challenger SPA OpenId Client",

                    AllowedGrantTypes = GrantTypes.Code,

                    RequireConsent = false,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,

                    // ClientUri = globalUrl,
                    // RedirectUris =
                    // {
                    //     globalUrl,
                    //     $"{globalUrl}/signin-oidc",
                    //     $"{globalUrl}/silentrenew"
                    // },
                    // PostLogoutRedirectUris = { globalUrl },
                    // AllowedCorsOrigins = { globalUrl },
                    ClientUri = reactClientUrl,
                    RedirectUris =
                    {
                        reactClientUrl,
                        $"{reactClientUrl}/signin-oidc",
                        $"{reactClientUrl}/silentrenew"
                    },
                    PostLogoutRedirectUris = { reactClientUrl },
                    AllowedCorsOrigins = { reactClientUrl },

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "challenge-catalog.bff", "paypal-payment.bff",
                        "challenge-order.bff", "challenges-temporary-storage.bff",
                        RoleClaim
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
                        "challenge-catalog.bff"
                    }
                },
                new Client
                {
                    ClientId = "challengeorderswaggerui",
                    ClientName = "Challenge Order",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { $"{challengeOrderUrl}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{challengeOrderUrl}/swagger/" },

                    AllowedScopes =
                    {
                        "challenge-order.bff"
                    }
                },
                new Client
                {
                    ClientId = "paymentswaggerui",
                    ClientName = "Payment",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { $"{paymentUrl}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{paymentUrl}/swagger/" },
                    AllowedScopes =
                    {
                        "paypal-payment.bff",
                        "paypal-payment.execute"
                    }
                },
                new Client
                {
                    ClientId = "challengestemporarystorageswaggerui",
                    ClientName = "Challenges Temporary Storage",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { $"{challengesTemporaryStorageUrl}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{challengesTemporaryStorageUrl}/swagger/" },
                    AllowedScopes =
                    {
                        "challenges-temporary-storage.bff"
                    }
                },
            };
        }

        private static IConfiguration GetConfiguration() => Helpers.ConfigurationProvider.GetConfiguration(Directory.GetCurrentDirectory());
    }
}

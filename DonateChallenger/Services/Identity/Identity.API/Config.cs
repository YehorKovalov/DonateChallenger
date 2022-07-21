using System.IO;

namespace Identity.API
{
    public static class Config
    {
        private static IConfiguration _configuration;
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = JwtClaimTypes.Role,
                UserClaims = new List<string> { JwtClaimTypes.Role }
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
                UserClaims = new List<string> { JwtClaimTypes.Role }
            },
            new ApiResource
            {
                Name = "challenge-order",
                DisplayName = "Challenge Order",
                Scopes = new List<string> { "challenge-order.bff" },
                ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                UserClaims = new List<string> { JwtClaimTypes.Role }
            },
            new ApiResource
            {
                Name = "payment",
                DisplayName = "Payment",
                Scopes = new List<string> { "paypal-payment.bff", "paypal-payment.execute" },
                ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                UserClaims = new List<string> { JwtClaimTypes.Role }
            },
            new ApiResource
            {
                Name = "challenges-temporary-storage",
                DisplayName = "Challenges Temporary Storage",
                Scopes = new List<string> { "challenge-temporary-storage.bff" },
                ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                UserClaims = new List<string> { JwtClaimTypes.Role }
            },
            new ApiResource
            {
                Name = "comment",
                DisplayName = "Comment",
                Scopes = new List<string> { "comment.bff", "comment.manager" },
                ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                UserClaims = new List<string> { JwtClaimTypes.Role }
            },
            new ApiResource
            {
                Name = "identity",
                DisplayName = "Comment",
                Scopes = new List<string> { "streamer-profile.bff", "user-profile.bff" },
                ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                UserClaims = new List<string> { JwtClaimTypes.Role }
            },
        };

        public static IEnumerable<ApiScope> Scopes => new List<ApiScope>
        {
            new ApiScope("challenge-catalog.bff", "Challenge Catalog BFF") { UserClaims = { JwtClaimTypes.Role } },
            new ApiScope("challenge-order.bff", "Challenge Order BFF") { UserClaims = { JwtClaimTypes.Role } },
            new ApiScope("paypal-payment.bff", "Paypal Payment BFF") { UserClaims = { JwtClaimTypes.Role} },
            new ApiScope("challenges-temporary-storage.bff", "Challenges Temporary Storage BFF") { UserClaims = { JwtClaimTypes.Role } },
            new ApiScope("comment.bff", "Comment BFF") { UserClaims = { JwtClaimTypes.Role } },
            new ApiScope("comment.manager", "Comment Manager") { UserClaims = { JwtClaimTypes.Role } },
            new ApiScope("streamer-profile.bff", "Streamer Profile BFF") { UserClaims = { JwtClaimTypes.Role } },
            new ApiScope("user-profile.bff", "User Profile BFF") { UserClaims = { JwtClaimTypes.Role } },
        };

        public static IEnumerable<Client> GetClients()
        {
            _configuration = GetConfiguration();
            var reactClientUrl = _configuration?["ReactClientUrl"] ?? throw new ArgumentNullException();
            var challengeCatalogUrl = _configuration?["ChallengeCatalogUrl"] ?? throw new ArgumentNullException();
            var challengeOrderUrl = _configuration?["ChallengeOrderUrl"] ?? throw new ArgumentNullException();
            var paymentUrl = _configuration?["PaymentUrl"] ?? throw new ArgumentNullException();
            var commentUrl = _configuration?["CommentUrl"] ?? throw new ArgumentNullException();
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
                        "comment.bff", "comment.manager", "streamer-profile.bff",
                        "user-profile.bff",
                        JwtClaimTypes.Role
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
                new Client
                {
                    ClientId = "commentswaggerui",
                    ClientName = "Comment",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { $"{commentUrl}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{commentUrl}/swagger/" },
                    AllowedScopes =
                    {
                        "comment.bff",
                        "comment.manager",
                    }
                },
            };
        }

        private static IConfiguration GetConfiguration() => Helpers.ConfigurationProvider.GetConfiguration(Directory.GetCurrentDirectory());
    }
}

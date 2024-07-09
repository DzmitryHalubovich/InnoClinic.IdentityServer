using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using static System.Net.WebRequestMethods;

namespace DuendeInMemoryTemplate;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("offices.api"),
        };

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("officeApi")
        {
            Scopes = new List<string> { "offices.api" },
            ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
            UserClaims = new List<string> { "role" }
        }
    };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "office.api" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "mvc",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:7057/signin-oidc" },

                PostLogoutRedirectUris = { "https://localhost:7057/signout-callback-oidc" },

                //FrontChannelLogoutUri = "https://localhost:7057/signout-oidc",

                AllowedScopes = 
                { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "office.api", 
                }
            },
            new Client
            {
                ClientId = "mvc_client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedScopes = { "offices.api" }
            },
            new Client
            {
                ClientId = "interactive_web",
                ClientName = "Interactive Web App",
                AllowedGrantTypes = GrantTypes.Hybrid,

                RequirePkce = false,
                AllowRememberConsent = false,

                RedirectUris = { "https://localhost:5003/signin-oidc" }, // client_url/signin-oidc

                PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },

                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "offices.api"
                }
            }
        };
}

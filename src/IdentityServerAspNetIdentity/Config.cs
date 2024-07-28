using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServerAspNetIdentity;

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
            new ApiScope("profiles.api"),
            new ApiScope("services.api"),
            new ApiScope("appointments.api"),
            new ApiScope("documents.api"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static IEnumerable<ApiResource> ApiResources => new[]
{
        new ApiResource("officeApi")
        {
            Scopes = new List<string> { "offices.api" },
            ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
            UserClaims = new List<string> { "role" }
        },
        new ApiResource("profilesApi")
        {
            Scopes = new List<string> { "profiles.api" },
            ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
            UserClaims = new List<string> { "role" }
        },
        new ApiResource("servicesApi")
        {
            Scopes = new List<string> { "services.api" },
            ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
            UserClaims = new List<string> { "role" }
        },
        new ApiResource("appointmentsApi")
        {
            Scopes = new List<string> { "appointments.api" },
            ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
            UserClaims = new List<string> { "role" }
        },
        new ApiResource("documentsApi")
        {
            Scopes = new List<string> { "documents.api" }
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

                AllowedScopes = 
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "offices.api",
                    "profiles.api",
                    "services.api",
                    "appointments.api",
                    "documents.api"
                }
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
                    IdentityServerConstants.LocalApi.ScopeName,
                    "offices.api"
                }
            },
            new Client
            {
                ClientId = "admin_blazor_client",
                ClientName = "Admin blazor client",
                AllowedGrantTypes = GrantTypes.Code,

                RequirePkce = true,
                RequireClientSecret = false,
                AllowedCorsOrigins = { "https://localhost:11001" },

                RedirectUris = { "https://localhost:11001/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:11001/signout-callback-oidc" },

                ClientSecrets = { new Secret("7F9BB006-EE7D-4969-9715-F217748B8DF1".Sha256()) },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    //IdentityServerConstants.LocalApi.ScopeName,
                    "offices.api",
                    "profiles.api",
                    "services.api",
                    "appointments.api"
                }
            }
        };
}

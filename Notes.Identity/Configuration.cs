using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Notes.Identity
{
    public class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("NotesWebApi", "Web API"),
                new ApiScope("notesTestapi", "Test API")
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("NotesWebApi", "Web API")
                {
                    Scopes = { "NotesWebApi" }
                },
                 new ApiResource("notesTestapi", "Test API")
                {
                    Scopes = { "NotesWebApi" }

                }
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                ClientId = "notes-web-api",
                ClientName = "Notes Web",
                ClientSecrets = {new Secret( "notesWebapp_secret".ToSha256())},
                AllowedGrantTypes = GrantTypes.Code,
               // RequireClientSecret = true,
                //RequirePkce  =true,
                RedirectUris =
                    {
                        "https://localhost:7227/signin-oidc", "http://localhost:44322/signin-oidc"
                    },
                AllowedCorsOrigins =
                    {
                        "https://localhost:7227"
                    },
                PostLogoutRedirectUris =
                    {
                        "https://localhost:7227/signout-callback-oidc"
                    },
                AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "NotesWebApi",

                    },
               AllowAccessTokensViaBrowser = true,
               AlwaysIncludeUserClaimsInIdToken = true
                }
            };
    }
}

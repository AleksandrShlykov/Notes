﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections;

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
                new ApiResource("NotesWebApi", "Web API", new[] {JwtClaimTypes.Name})
                {
<<<<<<< Updated upstream
                    Scopes = {"NotesWebAPI"}
=======
                    Scopes = { "NotesWebApi" }
                },
                 new ApiResource("notesTestapi", "Test API")
                {
                    Scopes = { "notesTestapi" }
>>>>>>> Stashed changes
                }
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                ClientId = "notes-web-api",
                ClientName = "Notes Web",
                AllowedGrantTypes = GrantTypes.Code,
               // RequireClientSecret = true,
                //RequirePkce  =true,
                RedirectUris =
                    {
                        "http://localhost:3000/signin-oidc"
                    },
                AllowedCorsOrigins =
                    {
                        "http://localhost:3000"
                    },
                PostLogoutRedirectUris =
                    {
                        "http://localhost:3000/signout-oidc"
                    },
                AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
<<<<<<< Updated upstream
                        "NotesWebAPI"
                    },
                AllowAccessTokensViaBrowser = true
=======
                        "NotesWebApi",
                        "notesTestapi"
                    },
               AllowAccessTokensViaBrowser = true,
               AlwaysIncludeUserClaimsInIdToken = true
>>>>>>> Stashed changes
                }
            };
    }
}
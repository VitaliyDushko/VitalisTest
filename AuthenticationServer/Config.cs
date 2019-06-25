using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("bookstore", "Book Store")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "bookstoremvcclient",
                    ClientName = "Book Store MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,


                    ClientSecrets =
                    {
                        new Secret("billy".Sha256())
                    },
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "bookstore"
                    },

                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "bookstorecmdclient",
                    ClientName = "Book Store CMD Client",
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("BillizzzCMDTool".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "bookstore" }
                }
            };
        }
    }
}
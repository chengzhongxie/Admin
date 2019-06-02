using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServer4;

namespace Admin
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
               new ApiResource("api","my Api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId="MVC",
                    AllowedGrantTypes=GrantTypes.Implicit,
                    ClientSecrets={new Secret("secret".Sha256())},
                    RequireConsent=false,
                    RedirectUris={"http://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris={ "http://localhost:5001/signout-callback-oidc" },
                    AllowedScopes={
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="110",
                    Username="xcz@163.com",
                    Password="123456"
                }
            };
        }
    }
}

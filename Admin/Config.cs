using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServer4;
using System.Security.Claims;

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
                    ClientName="Mvc Client",
                    ClientUri="http://localhost:5001",
                    LogoUri="https://tse3-mm.cn.bing.net/th?id=OIP.xq1C2fmnSw5DEoRMC86vJwD6D6&w=198&h=189&c=7&o=5&pid=1.7",
                    AllowRememberConsent=true,
                    AllowedGrantTypes=GrantTypes.Implicit,
                    ClientSecrets={new Secret("secret".Sha256())},
                    RequireConsent=true,
                    RedirectUris={"http://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris={ "http://localhost:5001/signout-callback-oidc" },
                    AlwaysIncludeUserClaimsInIdToken=true,// 将用户相关信息夹在idtoken中去
                    AllowedScopes={
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
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
                    Password="123456",
                    Claims=new List<Claim>
                    {
                        new Claim("name","xcz@163.com"),
                        new Claim("website","www.baidu.com")
                    }
                }
            };
        }
    }
}

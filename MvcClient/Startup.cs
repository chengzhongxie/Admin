using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


namespace MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            services.AddAuthentication(opent =>
            {
                opent.DefaultScheme = "Cookies";
                opent.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", opend =>
            {
                opend.SignInScheme = "Cookies";
                opend.Authority = "http://localhost:5000";
                opend.RequireHttpsMetadata = false;             

                opend.ClientId = "MVC";
                opend.ClientSecret = "secret";
                opend.SaveTokens = true;

                //opend.GetClaimsFromUserInfoEndpoint = true;
                //opend.ClaimActions.MapJsonKey("sub", "sub");
                //opend.ClaimActions.MapJsonKey("preferred_username", "preferred_username");
                //opend.ClaimActions.MapJsonKey("avatar", "avatar");
                //opend.ClaimActions.MapCustomJson("role", jobj => jobj["role"].ToString());

                opend.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                opend.Scope.Add("offline_access");
                opend.Scope.Add("openid");
                opend.Scope.Add("profile");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

     
    }
}

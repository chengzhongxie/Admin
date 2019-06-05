using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Admin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Admin.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using IdentityServer4;
using IdentityServer4.Services;
using Admin.Services;
using IdentityServer4.EntityFramework;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace Admin
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
            string SqlConnection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(SqlConnection);
            });
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentity<ApplicationUser, ApplicationUserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()// 临时证书
                                                //.AddInMemoryClients(Config.GetClients())// 添加内存客户端
                                                //.AddInMemoryApiResources(Config.GetApiResources())// 添加内存API资源
                                                //.AddInMemoryIdentityResources(Config.GetIdentityResources())// 添加内存标识资源
                                                //.AddTestUsers(Config.GetTestUsers());// 添加测试用户
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseMySql(SqlConnection, sql => sql.MigrationsAssembly(migrationAssembly));
                    };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseMySql(SqlConnection, sql => sql.MigrationsAssembly(migrationAssembly));
                    };
                })
               .AddAspNetIdentity<ApplicationUser>()
               .Services.AddScoped<IProfileService, ProfileService>();


            ////services.Configure<CookiePolicyOptions>(options =>
            ////{
            ////    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            ////    options.CheckConsentNeeded = context => true;
            ////    options.MinimumSameSitePolicy = SameSiteMode.None;
            ////});
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = "/Account/Login";
            //    });

            // 不限制密码格式
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            services.AddScoped<ConsentService>();
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
            app.UseIdentityServer();

            InitIdentityServerDataBase(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                     template: "{controller=Home}/{action=Index}/{id?}");
                //template: "{controller=Account}/{action=Register}/{id?}");
            });
        }

        public void InitIdentityServerDataBase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                if (!configurationDbContext.Clients.Any())
                {
                    foreach (var item in Config.GetClients())
                    {
                        configurationDbContext.Clients.Add(item.ToEntity());
                    }
                }
                if (!configurationDbContext.ApiResources.Any())
                {
                    foreach (var item in Config.GetApiResources())
                    {
                        configurationDbContext.ApiResources.Add(item.ToEntity());
                    }
                }
                if (!configurationDbContext.IdentityResources.Any())
                {
                    foreach (var item in Config.GetIdentityResources())
                    {
                        configurationDbContext.IdentityResources.Add(item.ToEntity());
                    }
                }
                configurationDbContext.SaveChanges();
            }
        }
    }
}

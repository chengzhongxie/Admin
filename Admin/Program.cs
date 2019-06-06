using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Admin.Data;
using IdentityServer4.EntityFramework.DbContexts;

namespace Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build()
                .MigratDbContext<ApplicationDbContext>((contex, serverPropert) =>
            {
                new ApplicationDbContextSeed().SeedAsync(contex, serverPropert).Wait();
            }).MigratDbContext<PersistedGrantDbContext>((contex,serverPropert)=> { })
            .MigratDbContext<ConfigurationDbContext>((contex, serverPropert) => { })
            .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

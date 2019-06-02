using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Admin.Data
{
    public static class WebHostMigrationExtensions
    {
        public static IWebHost MigratDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> action) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var server = scope.ServiceProvider;
                var logger = server.GetRequiredService<ILogger<TContext>>();
                var context = server.GetService<TContext>();
                try
                {
                    context.Database.Migrate();
                    action(context, server);
                    logger.LogError($"执行DbContextP{typeof(TContext).Name} seed方法成功");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"执行DbContextP{typeof(TContext).Name} seed方法失败");
                }
            }
            return webHost;
        }
        
    }
}

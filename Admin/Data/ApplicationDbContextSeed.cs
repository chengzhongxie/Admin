using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Admin.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Data
{
    public class ApplicationDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider service)
        {
            if (!dbContext.Users.Any())
            {
                _userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
                var user = new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "admin@163.com",
                    NormalizedUserName = "admin"
                };
                var create = await _userManager.CreateAsync(user, "123456");
                if (!create.Succeeded)
                {
                    throw new Exception("初始默认用户失败！");
                }

            }
        }
    }
}

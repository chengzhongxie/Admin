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
        private RoleManager<ApplicationUserRole> _roleManager;

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider service)
        {
            if (!dbContext.Roles.Any())
            {
                _roleManager = service.GetRequiredService<RoleManager<ApplicationUserRole>>();
                var role = new ApplicationUserRole() { Name = "Admin", NormalizedName = "Admin" };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception("初始默认角色失败！");
                }
            }
            if (!dbContext.Users.Any())
            {
                _userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

                var user = new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "admin@163.com",
                    NormalizedUserName = "admin",
                    SecurityStamp = "admin",
                    Avatar = "https://tse3-mm.cn.bing.net/th?id=OIP.xq1C2fmnSw5DEoRMC86vJwD6D6&w=198&h=189&c=7&o=5&pid=1.7"
                };            
                var create = await _userManager.CreateAsync(user, "123456");
                await _userManager.AddToRoleAsync(user, "Admin");
                if (!create.Succeeded)
                {
                    throw new Exception("初始默认用户失败！");
                }

            }
        }
    }
}

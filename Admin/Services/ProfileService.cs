using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Admin.Models;
using System.Security.Claims;
using IdentityModel;

namespace Admin.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        private async Task<List<Claim>> GetClaimsFromUserAsync(ApplicationUser user)
        {
            var clsims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject,user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName,user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                clsims.Add(new Claim(JwtClaimTypes.Role, item));
            }
            if (!string.IsNullOrWhiteSpace(user.Avatar))
            {
                clsims.Add(new Claim("avatar", user.Avatar));
            }
            return clsims;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjecid = context.Subject.Claims.FirstOrDefault(m => m.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjecid);
            context.IssuedClaims =await GetClaimsFromUserAsync(user);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            /*
            此处可以实现用户锁死功能
            */
            #region 判断用户id是否一致
            context.IsActive = false;
            var subjecid = context.Subject.Claims.FirstOrDefault(m => m.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjecid);
            context.IsActive = user != null;
            #endregion
        }
    }
}

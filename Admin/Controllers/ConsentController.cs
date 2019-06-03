using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Services;

namespace Admin.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentController(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        private async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);// 通过请求url找到验证服务端
            if (request == null)
            {
                return null;
            }
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);// 查找启用的客户端
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);// 查找以启用的资源
            return CreateConsentViewModel(request, client, resources);
        }

        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request, Client client, Resources resources)
        {
            var vm = new ConsentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.AllowRememberConsent = client.AllowRememberConsent;
            vm.IdentityScopes = resources.IdentityResources.Select(i => CreateScopenViewModel(i));
            vm.ResourceScopes = resources.ApiResources.SelectMany(m => m.Scopes).Select(i => CreateScopenViewModel(i));
            return vm;
        }

        private ScopenViewModel CreateScopenViewModel(IdentityResource identityResource)
        {
            return new ScopenViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked = identityResource.Required,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopenViewModel CreateScopenViewModel(Scope identityResource)
        {
            return new ScopenViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked = identityResource.Required,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            var model = BuildConsentViewModel(returnUrl);
            if (model == null)
            {

            }
            return View();
        }
    }
}
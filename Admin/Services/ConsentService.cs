using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Admin.ViewModels;

namespace Admin.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        #region 逻辑处理
        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request, Client client, Resources resources, InputConsentViewModel model)
        {
            var selectedScopes = model?.ScopesConsented ?? Enumerable.Empty<string>();
            var remeberConsent = model?.RemeberConsent ?? true;
            var vm = new ConsentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.RemeberConsent = remeberConsent;
            vm.IdentityScopes = resources.IdentityResources.Select(i => CreateScopenViewModel(i, selectedScopes.Contains(i.Name) || model == null));
            vm.ResourceScopes = resources.ApiResources.SelectMany(m => m.Scopes).Select(i => CreateScopenViewModel(i, selectedScopes.Contains(i.Name) || model == null));
            return vm;
        }

        private ScopenViewModel CreateScopenViewModel(IdentityResource identityResource, bool check)
        {
            return new ScopenViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked = check || identityResource.Required,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopenViewModel CreateScopenViewModel(Scope identityResource, bool check)
        {
            return new ScopenViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked = check || identityResource.Required,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }
        #endregion

        public async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl, InputConsentViewModel model = null)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);// 通过请求url找到验证服务端
            if (request == null)
            {
                return null;
            }
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);// 查找启用的客户端
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);// 查找以启用的资源
            var vm = CreateConsentViewModel(request, client, resources, model);
            vm.ReturnUrl = returnUrl;
            return vm;
        }

        public async Task<ProcessConsentResult> PorcessConsent(InputConsentViewModel model)
        {
            ConsentResponse consent = null;
            var result = new ProcessConsentResult();
            if (model != null)
            {
                if (model.Button == "no")
                {
                    consent = ConsentResponse.Denied;// 拒绝授权
                }
                else if (model.Button == "yes")
                {
                    if (model.ScopesConsented != null && model.ScopesConsented.Any())
                    {
                        consent = new ConsentResponse
                        {
                            RememberConsent = model.RemeberConsent,
                            ScopesConsented = model.ScopesConsented
                        };
                    }
                    result.ValidationError = "请至少选中一个权限";
                }
            }
            if (consent != null)
            {
                var response = await _identityServerInteractionService.GetAuthorizationContextAsync(model.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(response, consent);// 通知用户同意
                result.RedirectUrl = model.ReturnUrl;
            }
            else
            {
                result.ConsentViewModel = await BuildConsentViewModel(model.ReturnUrl, model);
            }
            return result;
        }
    }
}

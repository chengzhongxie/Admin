using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class ConsentViewModel : InputConsentViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 客户端显示名称（用于记录和同意屏幕）
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 有关客户端的更多信息的URI（在同意屏幕上使用）
        /// </summary>
        public string ClientUrl { get; set; }
        /// <summary>
        /// URI到客户端徽标（在同意屏幕上使用）
        /// </summary>
        public string ClientLogoUrl { get; set; }
        /// <summary>
        /// 指定用户是否可以选择存储同意决策（默认为true）
        /// </summary>
        //public bool AllowRememberConsent { get; set; }

        //public string ReturnUrl { get; set; }

        public IEnumerable<ScopenViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopenViewModel> ResourceScopes { get; set; }
    }
}

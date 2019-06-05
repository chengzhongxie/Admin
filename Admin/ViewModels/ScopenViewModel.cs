using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class ScopenViewModel
    {
        /// <summary>
        /// 标识资源的唯一名称。这是客户端将用于授权请求中的scope参数的值。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 该值将用于例如同意屏幕上。
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 该值将用于例如同意屏幕上。
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 指定同意屏幕是否会强调此范围（如果同意屏幕要实现此功能）。将此设置用于敏感或重要范围。默认为false。
        /// </summary>
        public bool Emphasize { get; set; }
        /// <summary>
        /// 指定用户是否可以在同意屏幕上取消选择范围（如果同意屏幕要实现此类功能）。默认为false。
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Checked { get; set; }
    }
}

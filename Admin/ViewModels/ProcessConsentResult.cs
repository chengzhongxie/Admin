using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class ProcessConsentResult
    {
        public string RedirectUrl { get; set; }
        public bool IsRedirect => RedirectUrl != null;
        public string ValidationError { get; set; }
        public ConsentViewModel ConsentViewModel { get; set; }
    }
}

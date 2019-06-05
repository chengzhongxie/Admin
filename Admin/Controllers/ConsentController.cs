using Admin.Services;
using Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class ConsentController : Controller
    {

        private readonly ConsentService _consentService;

        public ConsentController(ConsentService consentService)
        {
            _consentService = consentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await _consentService.BuildConsentViewModel(returnUrl);
            if (model == null)
            {

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConsentViewModel model)
        {
            var result = await _consentService.PorcessConsent(model);
            if (result.IsRedirect)
            {
                return Redirect(result.RedirectUrl);
            }
            if (!string.IsNullOrWhiteSpace(result.ValidationError))
            {
                ModelState.AddModelError("", result.ValidationError);
            }
            return View(result.ConsentViewModel);
        }
    }
}
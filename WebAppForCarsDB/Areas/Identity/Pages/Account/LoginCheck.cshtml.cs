using EFCarsDB.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebAppForCarsDB.Areas.Identity.Pages.Account
{
    public class LoginCheckModel : PageModel
    {

        private readonly SignInManager<WebAppForCarsDBUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        public string Authenticated { get; set; }

        public LoginCheckModel(SignInManager<WebAppForCarsDBUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public void OnGet()
        {
            Authenticated = (User.Identity.IsAuthenticated) ? "You are logged in." : "You're not.";
        }
    }
}

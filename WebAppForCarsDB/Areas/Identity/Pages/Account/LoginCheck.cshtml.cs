using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using EFCarsDB.Areas.Identity.Data;

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

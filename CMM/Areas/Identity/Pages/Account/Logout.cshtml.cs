using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CMM.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CMM.Controllers;

namespace CMM.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<CMMUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<CMMUser> _userManager;


        public LogoutModel(SignInManager<CMMUser> signInManager, ILogger<LogoutModel> logger, UserManager<CMMUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            TablesController tc = new TablesController();
            Guid rowKey = Guid.NewGuid();
            var userID = await _userManager.FindByEmailAsync(_userManager.GetUserName(User));
            string role = userID.userRoles;
            tc.testFunction(role, rowKey.ToString(), userID.Email, "Logged Out at: " + DateTime.Now);

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}

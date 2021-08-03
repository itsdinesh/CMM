using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CMM.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMM.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CMMUser> _signInManager;
        private readonly UserManager<CMMUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<CMMUser> userManager,
            SignInManager<CMMUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public SelectList GenderselectList = new SelectList
(
           new List<SelectListItem>
           {
            new SelectListItem {Selected = false, Text = "Male", Value="Male"},
            new SelectListItem {Selected = false, Text = "Female", Value = "Female" },

}, "Value", "Text", 1);

        public SelectList PatronRoleselectList = new SelectList
(
           new List<SelectListItem>
           {
            new SelectListItem { Selected = false, Text = "Patron", Value = "Patron" }

}, "Value", "Text", 1);

                public SelectList ManagerRoleselectList = new SelectList
        (
                  new List<SelectListItem>
                  {
                    new SelectListItem { Selected = false, Text = "Musician", Value = "Musician" }

        }, "Value", "Text", 1); 

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100)]
            [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$", ErrorMessage = "The password must have eight characters, at least one letter, one number and a special character.")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Please key in your name!")]
            [StringLength(200, ErrorMessage = "Enter your name with 6 - 200 chars", MinimumLength = 6)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Age")]
            [Range(18, 100, ErrorMessage = "We only accept 18 years old and above to be our member")]
            public int Age { get; set; }

            [Display(Name = "Gender")]
            public string Gender { get; set; }

               [Display(Name = "Group Name")]
               public string GroupName { get; set; }

               [Display(Name = "Address")]
               [RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "Only captial letter in the first char & only accept alphabet")]
               public string Address { get; set; }

              [Display(Name = "Role")]
              public string userRoles { get; set; } 
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new CMMUser { 
                    UserName = Input.Email, //table column refer the input from data
                    Email = Input.Email,
                    Name = Input.Name,
                    Age = Input.Age,
                    Gender = Input.Gender,
                    userRoles = Input.userRoles,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                var role = Roles.Patron.ToString();

                if (Input.userRoles == "Manager")
                {
                    role = Roles.Manager.ToString();
                }

                if (Input.userRoles == "Musician")
                {
                    role = Roles.Musician.ToString();
                }
                if (User.IsInRole("Manager") && result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    return LocalRedirect("/Admin/ListMusician");
                } else
                {
                    await _userManager.AddToRoleAsync(user, role);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

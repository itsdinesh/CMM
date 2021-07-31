using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CMM.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMM.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<CMMUser> _userManager;
        private readonly SignInManager<CMMUser> _signInManager;

        public IndexModel(
            UserManager<CMMUser> userManager,
            SignInManager<CMMUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel //form structure 2 rule, first - show the user id, second - disabled the edit function for name and id section
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public string User_id { get; set; }

            public string Name { get; set; } //lock from edit

            [Display(Name = "Age")]
            [Range(18, 100, ErrorMessage = "We only accept 18 years old and above to be our member")]
            public int Age { get; set; }

            public string Gender { get; set; }

        }

        private async Task LoadAsync(CMMUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel //fetch data from the table to the form
            {
                PhoneNumber = phoneNumber,
                Name = user.Name,
                Age = user.Age,
                Gender = user.Gender,
                User_id = user.Id

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid) //form have conflict / technical issue
            {
                await LoadAsync(user);
                StatusMessage = "Something Wrong in the form! We unable to update the data!";
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if(Input.Age != user.Age)
            {
                user.Age = Input.Age;
            }

            await _userManager.UpdateAsync(user);
           

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}

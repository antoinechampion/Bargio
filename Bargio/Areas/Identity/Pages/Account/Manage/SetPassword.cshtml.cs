using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bargio.Areas.Identity.Pages.Account.Manage
{
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;
        private readonly SignInManager<IdentityUserDefaultPwd> _signInManager;

        public SetPasswordModel(
            UserManager<IdentityUserDefaultPwd> userManager,
            SignInManager<IdentityUserDefaultPwd> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmer mot de passe")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Ne peut pas charger le num's '{_userManager.GetUserId(User)}'.");
            }

            if (!await _userManager.CheckPasswordAsync(user, IdentityUserDefaultPwd.DefaultPassword))
            {
                // L'utilisateur a déjà un mdp
                return RedirectToPage("./ChangePassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                NotFound($"Ne peut pas charger le num's '{_userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await _userManager.ChangePasswordAsync(user, IdentityUserDefaultPwd.DefaultPassword, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);

            return Redirect("/pg");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Bargio.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUserDefaultPwd> _signInManager;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUserDefaultPwd> signInManager,
            UserManager<IdentityUserDefaultPwd> userManager,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Tu dois rentrer ton num'ss")]
            public string Nums { get; set; }

            [Display(Name = "Password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null) {
            if (User.Identity.IsAuthenticated) {
                if (User.IsInRole("Admin")) {
                    return Redirect("/Admin/Quicknav");
                }
                else {
                    return Redirect("/pg");
                }
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Si mdp nul (champ vide), on remplace par une chaîne de caractères
            // vide pour pas faire bugger la bdd
            if (string.IsNullOrEmpty(Input.Password))
            {
                Input.Password = IdentityUserDefaultPwd.DefaultPassword;
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Nums, Input.Password, true, false);
                if (result.Succeeded)
                {
                    return Redirect("/pg");
                }
                else
                {
                    var user = await _userManager.FindByNameAsync(Input.Nums);
                    
                    if (user == null)
                        ModelState.AddModelError(string.Empty, "Le num's " + Input.Nums + " est inconnu");
                    else
                        ModelState.AddModelError(string.Empty, "Mot de passe invalide");
   
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

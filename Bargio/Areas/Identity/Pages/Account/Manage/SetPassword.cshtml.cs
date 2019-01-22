//          Bargio - SetPassword.cshtml.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Bargio.Data;
using BCrypt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Areas.Identity.Pages.Account.Manage
{
    public class SetPasswordModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUserDefaultPwd> _signInManager;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public SetPasswordModel(
            UserManager<IdentityUserDefaultPwd> userManager,
            SignInManager<IdentityUserDefaultPwd> signInManager,
            ApplicationDbContext context) {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty] public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Ne peut pas charger le num's '{_userManager.GetUserId(User)}'.");

            if (!await _userManager.CheckPasswordAsync(user, IdentityUserDefaultPwd.DefaultPassword))
                return RedirectToPage("./ChangePassword");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) NotFound($"Ne peut pas charger le num's '{_userManager.GetUserId(User)}'.");

            var addPasswordResult =
                await _userManager.ChangePasswordAsync(user, IdentityUserDefaultPwd.DefaultPassword, Input.NewPassword);
            if (!addPasswordResult.Succeeded) {
                foreach (var error in addPasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            // Si impossible de trouver l'utilisateur dans la BDD UserData, on annule le changement de mdp
            var userData = _context.UserData.Find(user.UserName);
            if (userData == null) {
                addPasswordResult =
                    await _userManager.ChangePasswordAsync(user, Input.NewPassword,
                        IdentityUserDefaultPwd.DefaultPassword);
                ModelState.AddModelError(string.Empty, "Impossible de trouver l'utilisateur dans la BDD UserData.");
                foreach (var error in addPasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            userData.DateDerniereModif = DateTime.Now;
            userData.FoysApiPasswordSalt = BCryptHelper.GenerateSalt();
            userData.FoysApiPasswordHash = BCryptHelper.HashPassword(Input.NewPassword, userData.FoysApiPasswordSalt);
            userData.FoysApiHasPassword = true;
            _context.Attach(userData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);

            return Redirect("/pg");
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Tu dois rentrer un mot de passe")]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmer mot de passe")]
            [Compare("NewPassword", ErrorMessage = "Le mot de passe et sa confirmation ne correspondent pas")]
            public string ConfirmPassword { get; set; }
        }
    }
}
﻿//          Bargio - ChangePassword.cshtml.cs
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
using Microsoft.Extensions.Logging;

namespace Bargio.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly SignInManager<IdentityUserDefaultPwd> _signInManager;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public ChangePasswordModel(
            UserManager<IdentityUserDefaultPwd> userManager,
            SignInManager<IdentityUserDefaultPwd> signInManager,
            ILogger<ChangePasswordModel> logger,
            ApplicationDbContext context) {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty] public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Ne peut pas charger le num's '{_userManager.GetUserId(User)}'.");

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (await _userManager.CheckPasswordAsync(user, IdentityUserDefaultPwd.DefaultPassword))
                return RedirectToPage("./SetPassword");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (string.IsNullOrEmpty(Input.NewPassword)) {
                Input.NewPassword = IdentityUserDefaultPwd.DefaultPassword;
                Input.ConfirmPassword = IdentityUserDefaultPwd.DefaultPassword;
            }

            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Ne peut pas charger le num's '{_userManager.GetUserId(User)}'.");

            var changePasswordResult =
                await _userManager.ChangePasswordAsync(user, Input.OldPassword.ToLower(), Input.NewPassword.ToLower());
            if (!changePasswordResult.Succeeded) {
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            // Si impossible de trouver l'utilisateur dans la BDD UserData, on annule le changement de mdp
            var userData = _context.UserData.Find(user.UserName);
            if (userData == null) {
                changePasswordResult =
                    await _userManager.ChangePasswordAsync(user, Input.NewPassword.ToLower(), Input.OldPassword.ToLower());
                ModelState.AddModelError(string.Empty, "Impossible de trouver l'utilisateur dans la BDD UserData.");
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            userData.FoysApiHasPassword = Input.NewPassword.ToLower() != IdentityUserDefaultPwd.DefaultPassword;
            
            userData.DateDerniereModif = DateTime.Now;
            userData.FoysApiPasswordSalt = BCryptHelper.GenerateSalt();
            userData.FoysApiPasswordHash = BCryptHelper.HashPassword(Input.NewPassword.ToLower(), userData.FoysApiPasswordSalt);
            _context.Attach(userData).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(user);

            return Redirect("/pg");
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Tu dois rentrer ton mot de passe actuel")]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe actuel")]
            public string OldPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Nouveau mot de passe")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmez le nouveau mot de passe")]
            [Compare("NewPassword", ErrorMessage =
                "Le nouveau mot de passe et la confirmation ne sont pas similaires.")]
            public string ConfirmPassword { get; set; }
        }
    }
}
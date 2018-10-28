using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bargio.Data;
using Bargio.Models;
using Bargio.Areas.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bargio.Areas.Admin.Pages.EditDatabase.Utilisateurs
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public UserData UserData { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }
        [BindProperty]
        public string SuccessMessage { get; set; }

        private async Task<bool> CreateIdentityUser(string userName) {
            var user = new IdentityUserDefaultPwd { UserName = userName };
            var result = await _userManager.CreateAsync(user, IdentityUserDefaultPwd.DefaultPassword);
            if (result.Succeeded)
            {
                return true;
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return false;
        }

        public IActionResult OnGet()
        {
            // Valeurs par défaut pour le modèle
            var tbk = "Li";
            var proms = (uint)(200 + DateTime.Now.Year - 2000);
            var listeNums = _context.UserData
                           .Where(o => o.TBK == "Li" && o.Proms == proms);
            var nums = listeNums.Any() ? listeNums.Max(o => o.Nums) + 1 : 1;

            UserData = new UserData
            {
                Nums = nums,
                TBK = tbk,
                Proms = proms,
                HorsFoys = false
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            ErrorMessage = SuccessMessage = null;

            UserData.UserName = UserData.Nums + UserData.TBK + UserData.Proms;
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_context.UserData.Any(o => o.UserName == UserData.UserName)) {
                ErrorMessage = "Cet utilisateur existe déjà !";
                return Page();
            }

            _context.UserData.Add(UserData);
            var success = await CreateIdentityUser(UserData.UserName);
            if (!success)
            {
                ErrorMessage = "Impossible de créer un utilisateur Identity...";
                _context.UserData.Remove(UserData);
                return Page();
            }

            SuccessMessage = "Nouvel utilisateur créé : " + UserData.UserName;
            await _context.SaveChangesAsync();
            ModelState.Clear();
            UserData.Nums++;
            return Page();
        }
    }
}
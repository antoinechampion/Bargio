using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Areas.User.Pages
{
    public class ChangerSurnomModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        [BindProperty] public string AncienSurnom { get; set; }
        [BindProperty] public string NouveauSurnom { get; set; }

        public ChangerSurnomModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            AncienSurnom = _context.UserData.First(o => o.UserName == user.UserName).Surnom;
            NouveauSurnom = "";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userData = _context.UserData.First(o => o.UserName == user.UserName);
            AncienSurnom = NouveauSurnom ?? AncienSurnom;
            userData.Surnom = AncienSurnom;
            userData.DateDerniereModif = DateTime.Now; // Pour la synchro sur la babasse

            _context.Attach(userData).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Page();
        }
    }
}
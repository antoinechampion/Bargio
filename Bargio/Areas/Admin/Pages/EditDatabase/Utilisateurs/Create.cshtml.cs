using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.Utilisateurs
{
    public class CreateModel : PageModel
    {
        private readonly Bargio.Data.ApplicationDbContext _context;

        public CreateModel(Bargio.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()        
        {
            // Valeurs par défaut pour le modèle
            UserData = new UserData
            {
                TBK = "Li",
                Proms = (uint)(200 + DateTime.Now.Year - 2000),
                HorsFoys = false
            };
            return Page();
        }

        [BindProperty]
        public UserData UserData { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            UserData.UserName = UserData.Nums + UserData.TBK + UserData.Proms;
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.UserData.Add(UserData);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.Utilisateurs
{
    public class DeleteModel : PageModel
    {
        private readonly Bargio.Data.ApplicationDbContext _context;

        public DeleteModel(Bargio.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserData UserData { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserData = await _context.UserData.FirstOrDefaultAsync(m => m.UserName == id);

            if (UserData == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserData = await _context.UserData.FindAsync(id);

            if (UserData != null)
            {
                _context.UserData.Remove(UserData);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

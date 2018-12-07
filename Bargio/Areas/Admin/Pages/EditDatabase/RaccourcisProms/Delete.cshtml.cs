using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.RaccourcisProms
{
    public class DeleteModel : PageModel
    {
        private readonly Bargio.Data.ApplicationDbContext _context;

        public DeleteModel(Bargio.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PromsKeyboardShortcut PromsKeyboardShortcut { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PromsKeyboardShortcut = await _context.PromsKeyboardShortcut.FirstOrDefaultAsync(m => m.Raccourci == id);

            if (PromsKeyboardShortcut == null)
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

            PromsKeyboardShortcut = await _context.PromsKeyboardShortcut.FindAsync(id);

            if (PromsKeyboardShortcut != null)
            {
                _context.PromsKeyboardShortcut.Remove(PromsKeyboardShortcut);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

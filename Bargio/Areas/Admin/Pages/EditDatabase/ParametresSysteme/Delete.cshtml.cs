using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.ParametresSysteme
{
    public class DeleteModel : PageModel
    {
        private readonly Bargio.Data.ApplicationDbContext _context;

        public DeleteModel(Bargio.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SystemParameters SystemParameters { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SystemParameters = await _context.SystemParameters.FirstOrDefaultAsync(m => m.IpServeur == id);

            if (SystemParameters == null)
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

            SystemParameters = await _context.SystemParameters.FindAsync(id);

            if (SystemParameters != null)
            {
                _context.SystemParameters.Remove(SystemParameters);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

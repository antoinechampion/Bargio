using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.HistoriqueTransactions
{
    public class EditModel : PageModel
    {
        private readonly Bargio.Data.ApplicationDbContext _context;

        public EditModel(Bargio.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransactionHistory TransactionHistory { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransactionHistory = await _context.TransactionHistory.FirstOrDefaultAsync(m => m.ID == id);

            if (TransactionHistory == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TransactionHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionHistoryExists(TransactionHistory.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TransactionHistoryExists(string id)
        {
            return _context.TransactionHistory.Any(e => e.ID == id);
        }
    }
}

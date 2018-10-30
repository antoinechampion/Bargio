using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.DemandesPaiement
{
    public class DeleteModel : PageModel
    {
        private readonly Bargio.Data.ApplicationDbContext _context;

        public DeleteModel(Bargio.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PaymentRequest PaymentRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PaymentRequest = await _context.PaymentRequest.FirstOrDefaultAsync(m => m.ID == id);

            if (PaymentRequest == null)
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

            PaymentRequest = await _context.PaymentRequest.FindAsync(id);

            if (PaymentRequest != null)
            {
                _context.PaymentRequest.Remove(PaymentRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

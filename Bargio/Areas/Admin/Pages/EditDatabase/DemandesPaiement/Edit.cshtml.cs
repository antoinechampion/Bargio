//          Bargio - Edit.cshtml.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System.Linq;
using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Areas.Admin.Pages.EditDatabase.DemandesPaiement
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public PaymentRequest PaymentRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(string id) {
            if (id == null) return NotFound();

            PaymentRequest = await _context.PaymentRequest.FirstOrDefaultAsync(m => m.ID == id);

            if (PaymentRequest == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) return Page();

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!PaymentRequestExists(PaymentRequest.ID))
                    return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool PaymentRequestExists(string id) {
            return _context.PaymentRequest.Any(e => e.ID == id);
        }
    }
}
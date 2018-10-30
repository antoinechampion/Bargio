using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.HistoriqueTransactions
{
    public class DetailsModel : PageModel
    {
        private readonly Bargio.Data.ApplicationDbContext _context;

        public DetailsModel(Bargio.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}

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
    public class IndexModel : PageModel
    {
        private readonly Bargio.Data.ApplicationDbContext _context;

        public IndexModel(Bargio.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PromsKeyboardShortcut> PromsKeyboardShortcut { get;set; }

        public async Task OnGetAsync()
        {
            PromsKeyboardShortcut = await _context.PromsKeyboardShortcut.ToListAsync();
        }
    }
}

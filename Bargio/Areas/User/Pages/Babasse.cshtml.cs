using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bargio.Areas.User.Pages
{
    public class BabasseModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty] public List<PromsKeyboardShortcut> RaccourcisProms { get; set; }

        public BabasseModel(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult OnGet() {
            RaccourcisProms = _context.PromsKeyboardShortcut.OrderBy(o => o.Proms).ToList();

            return Page();
        }
    }
}
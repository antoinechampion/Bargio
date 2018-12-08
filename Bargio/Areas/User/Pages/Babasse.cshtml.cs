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
        [BindProperty] public List<Product> Consommations { get; set; }

        public BabasseModel(ApplicationDbContext context) {
            _context = context;
        }

        private int ProductShortcutToInt(string shortcut) {
            if (int.TryParse(shortcut.Substring(1), out var raccourci))
                return raccourci;
            else
                return int.MaxValue;
        }
        public class ProductComparer : IComparer<Product>
        {
            public int Compare(Product p1, Product p2)
            {
                //return positive if a should be higher, return negative if b should be higher
                var success = int.TryParse(p1.RaccourciClavier.Substring(1), out var raccourci1);
                if (!success) raccourci1 = int.MaxValue;
                success = int.TryParse(p2.RaccourciClavier.Substring(1), out var raccourci2);
                if (!success) raccourci2 = int.MaxValue;
                return raccourci1 > raccourci2 ? 1 : -1;
            }
        }

        public int RaccourciToInt(string raccourci) {
            return int.TryParse(raccourci?.Substring(1), out var intRaccourci) ? intRaccourci : int.MaxValue;
        }
        public IActionResult OnGet() {
            RaccourcisProms = _context.PromsKeyboardShortcut.OrderBy(o => o.Proms).ToList();
            Consommations = _context.Product.OrderBy(o => RaccourciToInt(o.RaccourciClavier)).ToList();

            return Page();
        }
    }
}
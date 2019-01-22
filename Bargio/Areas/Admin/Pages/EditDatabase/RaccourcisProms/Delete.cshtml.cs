//          Bargio - Delete.cshtml.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Areas.Admin.Pages.EditDatabase.RaccourcisProms
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public PromsKeyboardShortcut PromsKeyboardShortcut { get; set; }

        public async Task<IActionResult> OnGetAsync(string id) {
            if (id == null) return NotFound();

            PromsKeyboardShortcut = await _context.PromsKeyboardShortcut.FirstOrDefaultAsync(m => m.ID == id);

            if (PromsKeyboardShortcut == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id) {
            if (id == null) return NotFound();

            PromsKeyboardShortcut = await _context.PromsKeyboardShortcut.FindAsync(id);

            if (PromsKeyboardShortcut != null) {
                _context.PromsKeyboardShortcut.Remove(PromsKeyboardShortcut);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
//          Bargio - Index.cshtml.cs
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

namespace Bargio.Areas.Admin.Pages.EditDatabase.ParametresSysteme
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty] public SystemParameters SystemParameters { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            SystemParameters = await _context.SystemParameters.FirstAsync();

            if (SystemParameters == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) return Page();

            // On del et on recrée plutôt que d'éditer à la volée :
            // Ca évite les comportements étranges et autres 
            // dbconcurrencyexception
            var systemParameters = _context.SystemParameters;
            foreach (var entity in systemParameters)
                systemParameters.Remove(entity);
            await _context.SaveChangesAsync();

            await systemParameters.AddAsync(SystemParameters);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
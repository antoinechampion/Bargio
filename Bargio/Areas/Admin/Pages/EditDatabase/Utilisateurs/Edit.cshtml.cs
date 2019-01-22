//          Bargio - Edit.cshtml.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Areas.Admin.Pages.EditDatabase.Utilisateurs
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager) {
            _context = context;
        }

        [BindProperty] public UserData UserData { get; set; }

        [BindProperty] public decimal AncienSolde { get; set; }

        public async Task<IActionResult> OnGetAsync(string id) {
            if (id == null) return NotFound();

            UserData = await _context.UserData.FirstOrDefaultAsync(m => m.UserName == id);
            AncienSolde = UserData.Solde;

            if (UserData == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            UserData.DateDerniereModif = DateTime.Now;

            if (!ModelState.IsValid) return Page();

            _context.TransactionHistory.Add(new TransactionHistory {
                Commentaire = "Edition par un administrateur",
                Date = DateTime.Now,
                IdProduits = null,
                Montant = UserData.Solde - AncienSolde,
                UserName = UserData.UserName
            });

            try {
                _context.Attach(UserData).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UserDataExists(UserData.UserName))
                    return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool UserDataExists(string id) {
            return _context.UserData.Any(e => e.UserName == id);
        }
    }
}
//          Bargio - Delete.cshtml.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty] public UserData UserData { get; set; }

        [BindProperty] public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id) {
            if (id == null) return NotFound();

            UserData = await _context.UserData.FirstOrDefaultAsync(m => m.UserName == id);

            if (UserData == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id) {
            ErrorMessage = null;

            if (id == null) return NotFound();

            UserData = await _context.UserData.FindAsync(id);

            if (UserData != null) {
                _context.UserData.Remove(UserData);
                var user = await _userManager.FindByNameAsync(id);
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded) ErrorMessage = "Can't delete Identity user: " + user;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
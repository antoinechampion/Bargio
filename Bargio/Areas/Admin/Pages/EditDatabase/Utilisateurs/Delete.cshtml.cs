using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Identity;

namespace Bargio.Areas.Admin.Pages.EditDatabase.Utilisateurs
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public UserData UserData { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserData = await _context.UserData.FirstOrDefaultAsync(m => m.UserName == id);

            if (UserData == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id) {
            ErrorMessage = null;

            if (id == null)
            {
                return NotFound();
            }

            UserData = await _context.UserData.FindAsync(id);

            if (UserData != null)
            {
                _context.UserData.Remove(UserData);
                var user = await _userManager.FindByNameAsync(id);
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded) {
                    ErrorMessage = "Can't delete Identity user: " + user;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

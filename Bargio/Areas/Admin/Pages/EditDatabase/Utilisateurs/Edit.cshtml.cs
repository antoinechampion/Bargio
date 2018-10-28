using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Identity;

namespace Bargio.Areas.Admin.Pages.EditDatabase.Utilisateurs
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager)
        {
            _context = context;
        }
        [BindProperty]
        public UserData UserData { get; set; }

        [BindProperty]
        public string OldUserName { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserData = await _context.UserData.FirstOrDefaultAsync(m => m.UserName == id);
            OldUserName = id;

            if (UserData == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            UserData.DateDerniereModif = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return Page();
            }


            try
            {
                _context.Attach(UserData).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!UserDataExists(UserData.UserName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserDataExists(string id)
        {
            return _context.UserData.Any(e => e.UserName == id);
        }
    }
}

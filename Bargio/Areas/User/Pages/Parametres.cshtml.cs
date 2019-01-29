using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Areas.User.Pages
{
    public class ParametresModel : PageModel
    {
        private ApplicationDbContext _context;

        [BindProperty] public bool CompteVerrouille { get; set; }

        public ParametresModel(ApplicationDbContext context) {
            _context = context;
        }

        public void OnGet() {
            CompteVerrouille = _context.UserData.First(o => o.UserName == User.Identity.Name).CompteVerrouille;
        }

        public async Task<IActionResult> OnPost() {
            var user = _context.UserData.First(o => o.UserName == User.Identity.Name);
            
            user.CompteVerrouille = CompteVerrouille;
            user.DateDerniereModif = DateTime.Now;
            _context.Attach(user).State = EntityState.Modified;
            await _context.SaveChangesAsync(); 

            return Page();
        }
    }
}
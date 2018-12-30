using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Areas.User.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        public readonly UserManager<IdentityUserDefaultPwd> _userManager;
        public readonly ApplicationDbContext _context;

        [BindProperty] public UserData UserData { get; set; }
        [BindProperty] public string DernierBucquage { get; set; }
        [BindProperty] public string DernierBucquageDate { get; set; }

        public DashboardModel(UserManager<IdentityUserDefaultPwd> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            UserData = await _context.UserData.FindAsync(identityUser.UserName);
            try
            {
                var derniereTransaction = await _context.TransactionHistory.Where(o => o.UserName == identityUser.UserName)
                    .OrderByDescending(o => o.Date).FirstAsync();
                DernierBucquage = derniereTransaction.Commentaire;
                DernierBucquageDate = " le " + derniereTransaction
                                          .Date.ToString("dd/MM/yyyy à HH:mm:ss");
            }
            catch (InvalidOperationException)
            {
                DernierBucquage = "--";
            }
            
            return Page();
        }
    }
}

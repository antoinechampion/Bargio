//          Bargio - Dashboard.cshtml.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Areas.Admin.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public DashboardModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty] public SystemParameters SystemParameters { get; set; }

        public IList<Product> Product { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            if (!User.IsInRole("Admin"))
                return Redirect("/pg");
            Product = await _context.Product.ToListAsync();
            SystemParameters = await _context.SystemParameters.FirstAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) return Page();

            // On del et on recrée plutôt que d'éditer à la volée :
            // Ca évite les comportements étranges et autres 
            // dbconcurrencyexception
            var systemParameters = _context.SystemParameters;
            var old = systemParameters.First();

            if (old.MotDePasseZifoys != SystemParameters.MotDePasseZifoys) {
                var user = await _userManager.FindByNameAsync("admin");
                var babasse = await _userManager.FindByNameAsync("babasse");
                if (user != null) {
                    var result = await _userManager.ChangePasswordAsync(user, old.MotDePasseZifoys,
                        SystemParameters.MotDePasseZifoys);
                    if (!result.Succeeded) SystemParameters.MotDePasseZifoys = old.MotDePasseZifoys;
                    await _userManager.ChangePasswordAsync(babasse, old.MotDePasseZifoys,
                        SystemParameters.MotDePasseZifoys);
                }
            }
            
            foreach (var entity in systemParameters)
                systemParameters.Remove(entity);
            await _context.SaveChangesAsync();

            await systemParameters.AddAsync(SystemParameters);

            await _context.SaveChangesAsync();

            return Redirect("/Admin/Dashboard");
        }
    }
}
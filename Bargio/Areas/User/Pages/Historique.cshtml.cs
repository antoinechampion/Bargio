using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bargio.Areas.User.Pages
{
    public class HistoriqueModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public class TransactionAffichage
        {
            public string date { get; set; }
            public string commentaire { get; set; }
            public decimal montant { get; set; }
        }

        [BindProperty] public List<TransactionAffichage> Transactions { get; set; }

        [BindProperty] public decimal Montant { get; set; }

        public HistoriqueModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Transactions = _context.TransactionHistory
                .Where(o => o.UserName == user.UserName)
                .OrderBy(o => o.Date)
                .Select(o => new TransactionAffichage
                {
                    commentaire = o.Commentaire ?? "",
                    date = o.Date.ToString("dd/MM/yyyy HH:mm"),
                    montant = o.Montant
                })
                .ToList();

            Montant = -_context.UserData
                .Find(user.UserName).Solde;

            return Page();
        }
    }
}
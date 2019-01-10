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
        [BindProperty] public Dictionary<string, int> CompteurConsos { get; set; }

        [BindProperty] public string MotDesZifoys { get; set; }
        [BindProperty] public string Actualites { get; set; }

        public DashboardModel(UserManager<IdentityUserDefaultPwd> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            CompteurConsos = new Dictionary<string, int>();
        }

        public async Task<IActionResult> OnGet()
        {
            if (User.IsInRole("Admin")) {
                return Redirect("/Admin/Quicknav");
            }
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

            var transactions = _context.TransactionHistory.Where(o => o.UserName == identityUser.UserName)
                .Select(o => o.IdProduits);
                
            var compteurIdProduits = new Dictionary<string, int>();
            foreach (var transaction in transactions)
            {
                if (transaction == null) // Pas d'ID produit pour la transaction (ex remise en babasse)
                    continue;

                foreach (var produit in transaction.Split(";", StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!compteurIdProduits.ContainsKey(produit))
                        compteurIdProduits.Add(produit, 1);
                    else
                        compteurIdProduits[produit] += 1;
                }
            }

            foreach (var produit in compteurIdProduits)
            {
                try {
                    var nomProduit = _context.Product.Find(produit.Key).Nom;
                    CompteurConsos.Add(nomProduit, produit.Value);
                } catch {}
            }

            var systemParameters = await _context.SystemParameters.FirstAsync();
            MotDesZifoys = systemParameters.MotDesZifoys;
            Actualites = systemParameters.Actualites;

            return Page();
        }
    }
}

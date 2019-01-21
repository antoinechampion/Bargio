using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bargio.Api
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("derniersbucquages")]
        public string DerniersBucquages() {
            var linq = _context.TransactionHistory
                .OrderByDescending(o => o.Date)
                .Take(100)
                .Select(o => new [] {
                    o.Date.ToString(CultureInfo.CurrentCulture), 
                    o.UserName, 
                    o.Commentaire, 
                    o.Montant.ToString(CultureInfo.CurrentCulture)
                });
            return JsonConvert.SerializeObject(new {data = linq});
        }

        [HttpGet("modifiercompte")]
        public string ModifierCompte() {
            var linq = _context.UserData
                .OrderBy(o => o.UserName)
                .Select(o => new[] {
                    o.UserName,
                    o.Solde.ToString(CultureInfo.CurrentCulture),
                    o.DateDerniereModif.ToString(CultureInfo.CurrentCulture)
                });
            return JsonConvert.SerializeObject(new {data = linq});
        }

        [HttpGet("recupererinfocompte/{username}")]
        public string RecupererInfoCompte(string username) {
            var lowercaseUsername = username.ToLower();
            var linq = _context.UserData
                .First(o => o.UserName == lowercaseUsername);
            return JsonConvert.SerializeObject(new {data = linq});
        }

    }
}

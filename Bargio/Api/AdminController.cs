using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                    o.Surnom,
                    o.Prenom,
                    o.Nom
                });
            return JsonConvert.SerializeObject(new {data = linq});
        }

        [HttpPost("modifiercompte")]
        public async Task<ActionResult> ModifierComptePost(string json) {
            try {
                var newData = JsonConvert.DeserializeObject<UserData>(json);
                var user = _context.UserData.First(o => o.UserName == newData.UserName);
                user.DateDerniereModif = DateTime.Now;
                user.ModeArchi = newData.ModeArchi;
                user.HorsFoys = newData.HorsFoys;
                user.Nom = newData.Nom;
                user.Prenom = newData.Prenom;
                user.Surnom = newData.Surnom;
                user.Nums = newData.Nums.ToLower();
                user.TBK = newData.TBK.ToLower();
                user.Proms = newData.Proms.ToLower();

                var newUsername = user.Nums + user.TBK + user.Proms;

                if (newUsername == user.UserName) {
                    await _context.SaveChangesAsync();
                }
                else {
                    var identityUser = await _userManager.FindByNameAsync(user.UserName);
                    identityUser.UserName = user.UserName;
                    await _userManager.UpdateAsync(identityUser);

                    _context.UserData.Remove(user);
                    await _context.SaveChangesAsync();
                    user.UserName = newUsername;
                    _context.UserData.Add(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e) {
                return BadRequest(e.ToString());
            }

            return Ok();
        }

        [HttpPost("bucquagemanuel")]
        public async Task<ActionResult> BucquageManuel(string json) {
            try {
                dynamic bucquage = JObject.Parse(json);
                var userName = (string) bucquage.UserName;
                // En supposant que la locale du serveur est fr-FR
                var montant = decimal.Parse(((string) bucquage.Montant).Replace(".", ","));
                var commentaire = (string) bucquage.Commentaire;
                var user = _context.UserData.First(o => o.UserName == userName);

                user.Solde += montant;
                user.DateDerniereModif = DateTime.Now;

                await _context.TransactionHistory.AddAsync(new TransactionHistory {
                    Commentaire = commentaire,
                    Date = DateTime.Now,
                    IdProduits = null,
                    Montant = montant,
                    UserName = userName
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception e) {
                return BadRequest(e.ToString());
            }

            return Ok();
        }

        [HttpPost("supprimermdp")]
        public async Task<ActionResult> SupprimerMdp(string json) {
            try {
                dynamic bucquage = JObject.Parse(json);
                var userName = (string) bucquage.UserName;

                var user = _context.UserData.First(o => o.UserName == userName);

                user.FoysApiHasPassword = false;
                user.DateDerniereModif = DateTime.Now;

                var identityUser = await _userManager.FindByNameAsync(user.UserName);
                await _userManager.RemovePasswordAsync(identityUser);
                await _userManager.AddPasswordAsync(identityUser, IdentityUserDefaultPwd.DefaultPassword);
                
                    await _context.SaveChangesAsync();
                    await _userManager.UpdateAsync(identityUser);
            }
            catch (Exception e) {
                return BadRequest(e.ToString());
            }

            return Ok();
        }

        [HttpGet("recupererinfocompte/{username}")]
        public string RecupererInfoCompte(string username) {
            var lowercaseUsername = username.ToLower();
            var linq = _context.UserData
                .First(o => o.UserName == lowercaseUsername);
            return JsonConvert.SerializeObject(linq);
        }

        [HttpGet("historique/{debut}/{fin}")]
        public ActionResult Historique(string debut, string fin) {
            try {
                var debutDate = DateTime.Parse(debut);
                var finDate = DateTime.Parse(fin);
                var linq = _context.TransactionHistory
                    .Where(o => o.Date > debutDate && o.Date < finDate)
                    .OrderByDescending(o => o.Date)
                    .Select(o => new[] {
                        o.Date.ToString(CultureInfo.CurrentCulture),
                        o.UserName,
                        o.Commentaire,
                        o.Montant.ToString(CultureInfo.CurrentCulture)
                    });
                return Ok(JsonConvert.SerializeObject(new {data = linq}));
            }
            catch (Exception e) {
                return BadRequest(e.ToString());
            }
        }

        [HttpPost("modearchi")]
        public async Task<ActionResult> ModeArchi(string json) {
            try {
                dynamic data = JObject.Parse(json);
                var listeProms = ((string)data.proms).Split(",");
                var exclusive = (bool)data.exclusive;
                var remove = (bool)data.remove;
                if (listeProms == null) {
                    return BadRequest("Paramètres incorrects: proms");
                }

                IQueryable<UserData> users;
                if (exclusive)
                    users = _context.UserData.Where(o => listeProms.Any(proms => !o.UserName.Contains(proms)));
                else 
                    users = _context.UserData.Where(o => listeProms.Any(proms => o.UserName.Contains(proms)));

                foreach (var user in users) {
                    user.ModeArchi = !remove;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e) {
                return BadRequest(e.ToString());
            }

            return Ok();
        }

    }
}

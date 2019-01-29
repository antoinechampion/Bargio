//          Bargio - Foys.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bargio.Api
{
#if DEBUG
    [AllowAnonymous]
#endif
    [Route("api/[controller]")]
    public class FoysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public FoysController(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        // Retourne la liste complète des utilisateurs, avec leur surnom, leur solde,
        // le hash de leur mot de passe et le salt, et si ils sont hors foy's
        public string Get() {
            var userData = _context.UserData.Select(o => new {
                o.UserName, o.HorsFoys, o.ModeArchi, o.Surnom,
                o.Solde, o.FoysApiHasPassword, o.FoysApiPasswordHash, 
                o.FoysApiPasswordSalt, o.CompteVerrouille
            }).ToList();
            return JsonConvert.SerializeObject(userData);
        }

        [HttpGet("{datetime}")]
        // Retourne la liste des utilisateurs depuis une date indiquée en paramètre
        // au format dd-MM-yyyy HH:mm:ss
        public string Get(string datetime) {
            try {
                var dateTime = DateTime.ParseExact(datetime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                var userData = _context.UserData.Where(o => o.DateDerniereModif >= dateTime)
                    .Select(o => new {
                        o.UserName, o.HorsFoys, o.ModeArchi,
                        o.Solde, o.FoysApiHasPassword, o.FoysApiPasswordHash, 
                        o.FoysApiPasswordSalt, o.CompteVerrouille
                    }).ToList();
                return JsonConvert.SerializeObject(userData);
            }
            catch (FormatException) {
                return "Invalid datetime format. Was expecting dd-MM-yyyy HH:mm:ss.";
            }
        }

        // Retourne les 20 dernières entrées dans l'historique
        // de l'utilisateur spécifié
        [HttpGet("userhistory/{username}")]
        public string GetUserHistory(string username) {
            var lowercaseUsername = username.ToLower();
            var history = _context.TransactionHistory.Where(o => o.UserName == lowercaseUsername)
                .OrderByDescending(o => o.Date).Take(20)
                .Select(o => new {
                    o.Commentaire,
                    o.Montant,
                    Date = o.Date.ToString("dd/MM HH:mm")
                })
                .ToList();
            return JsonConvert.SerializeObject(history);
        }

        [HttpPost]
        [Route("history")]
        // Méthode post qui reçoit l'historique local de la babasse et le
        // fusionne avec celui du serveur
        public JsonResult PostHistory(string json) {
            var dateTimeFormat = "dd-MM-yyyy HH:mm:ss"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter {DateTimeFormat = dateTimeFormat};
            List<TransactionHistory> transactionHistory;
            // Gestion d'exception à améliorer
            try {
                transactionHistory =
                    JsonConvert.DeserializeObject<List<TransactionHistory>>(json, dateTimeConverter);
            }
            catch (Exception) {
                return Json(false);
            }

            // On applique les transactions sur chaque utilisateur
            foreach (var transaction in transactionHistory) {
                var result = _context.UserData.SingleOrDefault(o => o.UserName == transaction.UserName);
                if (result != null) result.Solde += transaction.Montant;
            }

            // On met à jour la BDD historique
            _context.TransactionHistory.AddRange(transactionHistory);

            _context.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        [Route("sethorsfoys")]
        public JsonResult SetHorsFoys(string json) {
            dynamic p = JObject.Parse(json);
            string userName = p.UserName;
            var result = _context.UserData.SingleOrDefault(o => o.UserName == userName);
            if (result != null) {
                result.HorsFoys = p.HorsFoys;
                result.DateDerniereModif = DateTime.Now;
                _context.SaveChanges();
                return Json(true);
            }

            return Json(false);
        }

        // Retourne les paramètres zifoy'ss de la babasse
        [HttpGet("zifoysparams")]
        public string GetZifoysParameters() {
            var parametres = _context.SystemParameters
                .Select(o => new {
                    o.MiseHorsBabasseAutoActivee,
                    o.MiseHorsBabasseSeuil,
                    o.MiseHorsBabasseInstantanee,
                    o.MiseHorsBabasseQuotidienne,
                    o.MiseHorsBabasseQuotidienneHeure,
                    o.MiseHorsBabasseHebdomadaireJours,
                    o.MiseHorsBabasseHebdomadaireHeure,
                    o.MotDePasseZifoys,
                    o.Snow,
                    o.MotDesZifoys,
                    o.Actualites
                })
                .First();
            return JsonConvert.SerializeObject(parametres);
        }

        // Retourne les paramètres zifoy'ss de la babasse
        [HttpPost]
        [Route("zifoysparams")]
        public async Task<JsonResult> SetZifoysParameters(string json) {
            dynamic p = JObject.Parse(json);
            var parameters = _context.SystemParameters.First();
            if (parameters.MotDePasseZifoys != (string) p.MotDePasseZifoys) {
                var user = await _userManager.FindByNameAsync("admin");
                if (user != null) {
                    var result = await _userManager.ChangePasswordAsync(user, parameters.MotDePasseZifoys,
                        (string) p.MotDePasseZifoys);
                    if (!result.Succeeded) parameters.MotDePasseZifoys = p.MotDePasseZifoys;
                }
            }

            parameters.MiseHorsBabasseAutoActivee = p.MiseHorsBabasseAutoActivee;
            parameters.MiseHorsBabasseSeuil = p.MiseHorsBabasseSeuil;
            parameters.MiseHorsBabasseInstantanee = p.MiseHorsBabasseInstantanee;
            parameters.MiseHorsBabasseQuotidienne = p.MiseHorsBabasseQuotidienne;
            parameters.MiseHorsBabasseQuotidienneHeure = p.MiseHorsBabasseQuotidienneHeure;
            parameters.MiseHorsBabasseHebdomadaireHeure = p.MiseHorsBabasseHebdomadaireHeure;
            parameters.MiseHorsBabasseHebdomadaireJours = p.MiseHorsBabasseHebdomadaireJours;
            parameters.MotDePasseZifoys = p.MotDePasseZifoys;
            parameters.MotDesZifoys = p.MotDesZifoys;
            parameters.Actualites = p.Actualites;
            parameters.Snow = p.Snow;

            var systemParameters = _context.SystemParameters;
            foreach (var entity in systemParameters)
                systemParameters.Remove(entity);
            await _context.SaveChangesAsync();

            await systemParameters.AddAsync(parameters);

            await _context.SaveChangesAsync();

            return Json(true);
        }

        // Supprime le mdp d'un utilisateur
        [HttpPost]
        [Route("supprimermdp")]
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
    }
}
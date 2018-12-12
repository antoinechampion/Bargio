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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public FoysController(ApplicationDbContext context) {
            _context = context;
        }
        
        [HttpGet]
        // Retourne la liste complète des utilisateurs, avec leur surnom, leur solde,
        // le hash de leur mot de passe et le salt, et si ils sont hors foy's
        public string Get() {
            var userData = _context.UserData.Select(o => new {o.UserName, o.HorsFoys, o.Surnom,
                o.Solde, o.FoysApiHasPassword, o.FoysApiPasswordHash, o.FoysApiPasswordSalt}).ToList();
            return JsonConvert.SerializeObject(userData);
        }
        
        [HttpGet("{datetime}")]
        // Retourne la liste des utilisateurs depuis une date indiquée en paramètre
        // au format dd-MM-yyyy HH:mm:ss
        public string Get(string datetime) {
            try {
                var dateTime = DateTime.ParseExact(datetime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                var userData = _context.UserData.Where(o => o.DateDerniereModif >= dateTime)
                    .Select(o => new {o.UserName, o.HorsFoys, 
                        o.Solde, o.FoysApiHasPassword, o.FoysApiPasswordHash, o.FoysApiPasswordSalt}).ToList();
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
                    Commentaire = o.Commentaire,
                    Montant = o.Montant,
                    Date = o.Date.ToString("d/MM HH:mm")
                })
                .ToList();
            return JsonConvert.SerializeObject(history);
        }
        
        [HttpPost]
        [Route("history")]
        // Méthode post qui reçoit l'historique local de la babasse et le
        // fusionne avec celui du serveur
        public void PostHistory(string json)
        {
            var transactionHistory = JsonConvert.DeserializeObject<List<TransactionHistory>>(json);

            // On applique les transactions sur chaque utilisateur
            foreach (var transaction in transactionHistory)
            {
                var result = _context.UserData.SingleOrDefault(o => o.UserName == transaction.UserName);
                if (result != null) {
                    result.Solde += transaction.Montant;
                }
            }

            // On met à jour la BDD historique
            _context.TransactionHistory.AddRange(transactionHistory);

            _context.SaveChanges();
        }

        // Retourne les paramètres zifoy'ss de la babasse
        [HttpGet("zifoysparams")]
        public string GetZifoysParameters() {
            var parametres = _context.SystemParameters
                .Select(o => new {
                    o.MiseHorsBabasseAutoActivee,
                    o.MiseHorsBabasseInstantanee,
                    o.MiseHorsBabasseQuotidienne,
                    o.MiseHorsBabasseQuotidienneHeure,
                    o.MiseHorsBabasseHebdomadaireJours,
                    o.MiseHorsBabasseHebdomadaireHeure
                })
                .ToList();
            return JsonConvert.SerializeObject(parametres);
        }

        // Retourne les paramètres zifoy'ss de la babasse
        [HttpPost]
        [Route("zifoysparams")]
        public void SetZifoysParameters(string json) {
            dynamic p = JsonConvert.DeserializeObject(json);
            var parameters = _context.SystemParameters.First();
            parameters.MiseHorsBabasseAutoActivee = p.MiseHorsBabasseAutoActivee;
            parameters.MiseHorsBabasseInstantanee = p.MiseHorsBabasseInstantanee;
            parameters.MiseHorsBabasseQuotidienne = p.MiseHorsBabasseQuotidienne;
            parameters.MiseHorsBabasseQuotidienneHeure = p.MiseHorsBabasseQuotidienneHeure;
            parameters.MiseHorsBabasseHebdomadaireHeure = p.MiseHorsBabasseHebdomadaireHeure;
            parameters.MiseHorsBabasseHebdomadaireJours = p.MiseHorsBabasseHebdomadaireJours;
            _context.Attach(parameters).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}

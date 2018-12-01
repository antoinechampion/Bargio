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
        public string Get() {
            var userData = _context.UserData.Select(o => new {o.UserName, o.HorsFoys, o.Surnom,
                o.Solde, o.FoysApiHasPassword, o.FoysApiPasswordHash, o.FoysApiPasswordSalt}).ToList();
            return JsonConvert.SerializeObject(userData);
        }
        
        [HttpGet("{datetime}")]
        public string Get(string datetime) {
            var dateTime = DateTime.ParseExact(datetime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            var userData = _context.UserData.Where(o => o.DateDerniereModif >= dateTime)
                .Select(o => new {o.UserName, o.HorsFoys, 
                    o.Solde, o.FoysApiHasPassword, o.FoysApiPasswordHash, o.FoysApiPasswordSalt}).ToList();
            return JsonConvert.SerializeObject(userData);
        }
        
        [HttpPost]
        [Route("history")]
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
    }
}

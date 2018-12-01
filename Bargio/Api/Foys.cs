using System;
using System.Collections.Generic;
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
            var userData = _context.UserData.Select(o => new {o.UserName, o.HorsFoys, 
                o.Solde, o.FoysApiHasPassword, o.FoysApiPasswordHash, o.FoysApiPasswordSalt}).ToList();
            return JsonConvert.SerializeObject(userData);
        }
        
        [HttpGet("{datetime}")]
        public string Get(DateTime datetime)
        {
            var userData = _context.UserData.Where(o => o.DateDerniereModif >= datetime)
                .Select(o => new {o.UserName, o.HorsFoys, 
                    o.Solde, o.FoysApiHasPassword, o.FoysApiPasswordHash, o.FoysApiPasswordSalt}).ToList();
            return JsonConvert.SerializeObject(userData);
        }
        
        [HttpPost]
        [Route("userdata")]
        public void PostUserData(string json)
        {
            var userData = JsonConvert.DeserializeObject<List<UserData>>(json);
            foreach (var user in userData)
            {
                var result = _context.UserData.SingleOrDefault(o => o.UserName == user.UserName);
                if (result != null) {
                    result.HorsFoys = user.HorsFoys;
                    result.Solde = user.Solde;
                }
            }

            _context.SaveChanges();
        }

        [HttpPost]
        [Route("history")]
        public void PostHistory(string json)
        {
            var transactionHistory = JsonConvert.DeserializeObject<List<TransactionHistory>>(json);
            _context.TransactionHistory.AddRange(transactionHistory);

            _context.SaveChanges();
        }
    }
}

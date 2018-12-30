using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Panneau de contrôle debug:
// https://homologation.lydia-app.com/index.php/backoffice/request/index

namespace Bargio.Api
{
    [AllowAnonymous]
    public class LydiaController : Controller
    {
        private ApplicationDbContext _context;

        public LydiaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("api/[controller]/confirm")]
        [HttpPost]
        public string Confirm(string order_ref)
        {
            var request = _context.PaymentRequest.Find(order_ref);
            if (request == null)
                return "Cette demande de paiement (" + order_ref + ") n'existe pas";

            var user = _context.UserData.FirstOrDefault(o => o.UserName == request.UserName);
            if (user == null)
                return "L'utilisateur associé à cette demande de paiement est introuvable : " + request.UserName;

            user.Solde += request.Montant;

            _context.TransactionHistory.Add(new TransactionHistory
            {
                UserName = user.UserName,
                Montant = request.Montant,
                Commentaire = "Remise en babasse en ligne"
            });

            _context.Attach(user).State = EntityState.Modified;
            _context.PaymentRequest.Remove(request);
            _context.SaveChanges();

            return "La demande de paiement " + order_ref + " a bien été confirmée";
        }

        [Route("api/[controller]/cancel")]
        [HttpPost]
        public string Cancel(string order_ref)
        {
            var request = _context.PaymentRequest.Find(order_ref);
            if (request == null)
                return "Cette demande de paiement (" + order_ref + ") n'existe pas";

            _context.PaymentRequest.Remove(request);
            _context.SaveChanges();

            return "La demande de paiement " + order_ref + " a bien été annulée";
        }
    }
}

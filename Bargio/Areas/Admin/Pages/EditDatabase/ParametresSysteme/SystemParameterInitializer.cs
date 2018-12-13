using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.User
{
    public class SystemParametersInitializer
    {
        public static async Task SeedData
            (ApplicationDbContext context) {

            var p = new SystemParameters {
                IpServeur = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString(),
                DerniereConnexionBabasse = DateTime.Now,
                BucquagesBloques = false,
                LydiaBloque = false,
                Maintenance = false,
                MiseHorsBabasseAutoActivee = false,
                MiseHorsBabasseInstantanee = false,
                MiseHorsBabasseQuotidienne = true,
                MiseHorsBabasseHebdomadaireHeure = "00:00",
                MiseHorsBabasseQuotidienneHeure = "00:00",
                MiseHorsBabasseHebdomadaireJours = "",
                MotDePasseZifoys = "zifoys"
            };

            if (context.SystemParameters.Any()) {
                var previous = context.SystemParameters.First();
                p.MiseHorsBabasseAutoActivee = previous.MiseHorsBabasseAutoActivee;
                p.MiseHorsBabasseInstantanee = previous.MiseHorsBabasseInstantanee;
                p.MiseHorsBabasseQuotidienne = !previous.MiseHorsBabasseInstantanee;
                p.MiseHorsBabasseQuotidienneHeure = previous.MiseHorsBabasseQuotidienneHeure ?? "00:00";
                p.MiseHorsBabasseHebdomadaireHeure = previous.MiseHorsBabasseHebdomadaireHeure ?? "00:00";
                p.MiseHorsBabasseHebdomadaireJours = previous.MiseHorsBabasseHebdomadaireJours ?? "";
                p.MotDePasseZifoys = previous.MotDePasseZifoys ?? "zifoys";
            }

            var systemParameters = context.SystemParameters;
            foreach (var entity in systemParameters)
                systemParameters.Remove(entity);
            await context.SaveChangesAsync();

            await systemParameters.AddAsync(p);

            await context.SaveChangesAsync();
        }
    }
}

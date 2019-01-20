using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Areas.Admin.Pages.EditDatabase.ParametresSysteme
{
    public class SystemParametersInitializer
    {
        public static async Task SeedData
            (ApplicationDbContext context) {

            var p = new SystemParameters {
                DerniereConnexionBabasse = DateTime.Now,
                BucquagesBloques = false,
                LydiaBloque = false,
                CommissionLydiaVariable = new decimal(1.5), // %
                CommissionLydiaFixe = new decimal(0.10), // €
                MinimumRechargementLydia = new decimal(10), // €
                Maintenance = false,
                MiseHorsBabasseAutoActivee = false,
                MiseHorsBabasseInstantanee = false,
                MiseHorsBabasseQuotidienne = true,
                MiseHorsBabasseHebdomadaireHeure = "00:00",
                MiseHorsBabasseQuotidienneHeure = "00:00",
                MiseHorsBabasseHebdomadaireJours = "",
                MotDePasseZifoys = "zifoys",
                Snow = true,
                MotDesZifoys = "L'endroit parfait pour caler des rhopses hyper ayat'sssssssss.",
                Actualites = "Jeudi 8 Novembre - Zbeuuuuuuul\n"
                    + "Vendredi 9 Novembre - Remise de fam'ss\n"
                    + "WBP - fin'ss - ce genre de choses..."
            };
            var systemParameters = context.SystemParameters;
            foreach (var entity in systemParameters)
                systemParameters.Remove(entity);
            await context.SaveChangesAsync();

            await systemParameters.AddAsync(p);

            await context.SaveChangesAsync();
        }
    }
}

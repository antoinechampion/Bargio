﻿//          Bargio - SystemParameterInitializer.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.ParametresSysteme
{
    public class SystemParametersInitializer
    {
        public static async Task SeedData
            (ApplicationDbContext context) {
            if (context.SystemParameters.Any()) {
                var p = context.SystemParameters.First();
                p.Maintenance = false;
                context.SystemParameters.Update(p);
                await context.SaveChangesAsync();
            }
            else {
                var p = new SystemParameters {
                    DerniereConnexionBabasse = DateTime.Now,
                    LydiaBloque = false,
                    LydiaToken = "5774f7d9df76f082807252",
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
                    MotDesZifoys = "",
                    Actualites = ""
                };

                await context.SystemParameters.AddAsync(p);
            }

            await context.SaveChangesAsync();
        }
    }
}
﻿using Bargio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.Utilisateurs
{
    public class UserDataInitializer
    {
        public static async Task SeedData
            (ApplicationDbContext context) {
            var userData = context.UserData;
            #if DEBUG
            if (!userData.Any(o => o.UserName == "1test217"))
                await userData.AddAsync(new UserData {
                    UserName = "1test217", 
                    Nums = "1",
                    TBK = "test",
                    Proms = "217",
                    HorsFoys = false, 
                    Solde = 0,
                    DateDerniereModif = DateTime.Now
                });

            if (!userData.Any(o => o.UserName == "2test217"))
                await userData.AddAsync(new UserData
                {
                    UserName = "2test217",
                    Nums = "2",
                    TBK = "test",
                    Proms = "217",
                    HorsFoys = false,
                    Solde = -13,
                    DateDerniereModif = DateTime.Now,
                    ModeArchi = true
                });
               #endif
            await context.SaveChangesAsync();
        }
    }
}

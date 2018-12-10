using Bargio.Data;
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
            if (!userData.Any(o => o.UserName == "1test217"))
                await userData.AddAsync(new UserData {
                    UserName = "1test217", 
                    Nums = "1",
                    TBK = "test",
                    Proms = "217",
                    HorsFoys = true, 
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
                    DateDerniereModif = DateTime.Now
                });

            if (!userData.Any(o => o.UserName == "3test217"))
                await userData.AddAsync(new UserData
                {
                    UserName = "3test217",
                    Nums = "3",
                    TBK = "test",
                    Proms = "217",
                    HorsFoys = false,
                    Solde = 0,
                    DateDerniereModif = DateTime.Now
                });

            if (!userData.Any(o => o.UserName == "4test217"))
                await userData.AddAsync(new UserData
                {
                    UserName = "4test217",
                    Nums = "4",
                    TBK = "test",
                    Proms = "217",
                    HorsFoys = false,
                    Solde = 10000,
                    DateDerniereModif = DateTime.Now
                });
            await context.SaveChangesAsync();
        }
    }
}

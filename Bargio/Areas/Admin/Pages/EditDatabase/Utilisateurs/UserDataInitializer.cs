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
            if (!userData.Any(o => o.UserName == "1Test217"))
                await userData.AddAsync(new UserData {
                    UserName = "1Test217", 
                    Nums = "1",
                    TBK = "Test",
                    Proms = "217",
                    HorsFoys = true, 
                    Solde = 0,
                    DateDerniereModif = DateTime.Now
                });

            if (!userData.Any(o => o.UserName == "2Test217"))
                await userData.AddAsync(new UserData
                {
                    UserName = "2Test217",
                    Nums = "2",
                    TBK = "Test",
                    Proms = "217",
                    HorsFoys = false,
                    Solde = -13,
                    DateDerniereModif = DateTime.Now
                });

            if (!userData.Any(o => o.UserName == "3Test217"))
                await userData.AddAsync(new UserData
                {
                    UserName = "3Test217",
                    Nums = "3",
                    TBK = "Test",
                    Proms = "217",
                    HorsFoys = false,
                    Solde = 0,
                    DateDerniereModif = DateTime.Now
                });

            if (!userData.Any(o => o.UserName == "4Test217"))
                await userData.AddAsync(new UserData
                {
                    UserName = "4Test217",
                    Nums = "4",
                    TBK = "Test",
                    Proms = "217",
                    HorsFoys = false,
                    Solde = 10000,
                    DateDerniereModif = DateTime.Now
                });
            await context.SaveChangesAsync();
        }
    }
}

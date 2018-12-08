using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.Consommations
{
    public class ProductsInitializer
    {
        public static async Task SeedData
            (ApplicationDbContext context) {
            var consos = context.Product;
            if (!consos.Any())
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler",
                    Prix = 0.80M
                });
            await context.SaveChangesAsync();
        }
    }
}

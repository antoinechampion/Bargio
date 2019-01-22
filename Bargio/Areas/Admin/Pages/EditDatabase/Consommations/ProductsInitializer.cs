//          Bargio - ProductsInitializer.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

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
            if (!consos.Any()) {
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F1",
                    Nom = "Jupiler/Illoise",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F2",
                    Nom = "Hoegarden",
                    Prix = 1.15M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F3",
                    Nom = "Alcool fort (1 dose)",
                    Prix = 0.50M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F4",
                    Nom = "Bière bout's",
                    Prix = 1.60M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F5",
                    Nom = "Bière F5",
                    Prix = 1.25M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F6",
                    Nom = "mdr",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F7",
                    Nom = "Pain choc's",
                    Prix = 0.70M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F8",
                    Nom = "mdr",
                    Prix = 0.80M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F9",
                    Nom = "Martini/Picon (1 dose)",
                    Prix = 0.35M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F10",
                    Nom = "Zabulle",
                    Prix = 0.70M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F11",
                    Nom = "mdr",
                    Prix = 0.35M
                });
                await consos.AddAsync(new Product {
                    RaccourciClavier = "F12",
                    Nom = "Chocolat chaud",
                    Prix = 0.35M
                });
                await consos.AddAsync(new Product {
                    Nom = "Quillon",
                    Prix = 1.00M
                });
                await consos.AddAsync(new Product {
                    Nom = "Thé",
                    Prix = 0.10M
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.HistoriqueTransactions
{
    public class TransactionHistoryInitializer
    {
        public static async Task SeedData
            (ApplicationDbContext context) {
#if !DEBUG
            return;
#endif

            if (!context.TransactionHistory.Any(o => o.UserName.ToLower() == "1test217"))
            {
                var r = new Random(1);
                var transactions = new List<TransactionHistory>();
                var date = DateTime.Now.AddYears(-1);
                var products = context.Product.Select(o => o.Id).ToList();
                while (date < DateTime.Now)
                {
                    transactions.Add(new TransactionHistory
                    {
                        UserName = "1test217",
                        Date = date,
                        Montant = r.Next(-10, 5),
                        Commentaire = "DB Seeding",
                        IdProduits = products[r.Next(products.Count)]
                    });
                    date = date.AddHours(6);
                }
                await context.TransactionHistory.AddRangeAsync(transactions);
            }

            await context.SaveChangesAsync();
        }
    }
}

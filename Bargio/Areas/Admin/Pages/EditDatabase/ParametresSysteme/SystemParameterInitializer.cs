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

            var systemParameters = context.SystemParameters;
            foreach (var entity in systemParameters)
                systemParameters.Remove(entity);

            await systemParameters.AddAsync(new SystemParameters {
                IpServeur = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString(),
                DerniereConnexionBabasse = DateTime.Now,
                BucquagesBloques = false,
                LydiaBloque = false
            });

            await context.SaveChangesAsync();
        }
    }
}

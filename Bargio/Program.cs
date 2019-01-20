using System;
using Bargio.Areas.Admin.Pages.EditDatabase.Consommations;
using Bargio.Areas.Admin.Pages.EditDatabase.HistoriqueTransactions;
using Bargio.Areas.Admin.Pages.EditDatabase.ParametresSysteme;
using Bargio.Areas.Admin.Pages.EditDatabase.RaccourcisProms;
using Bargio.Areas.Admin.Pages.EditDatabase.Utilisateurs;
using Bargio.Areas.Identity;
using Bargio.Areas.User;
using Bargio.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bargio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetService<UserManager<IdentityUserDefaultPwd>>();
                var roleManager = services.GetService<RoleManager<IdentityRole>>();
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                // requires using Microsoft.Extensions.Configuration;
                var config = host.Services.GetRequiredService<IConfiguration>();
                // Set password with the Secret Manager tool.
                // dotnet user-secrets set SeedUserPW <pw>
                

                #region Seed DB
                var logger = services.GetRequiredService<ILogger<Program>>();
                var testUserPw = config["SeedUserPW"];
                try {
                    UserDataInitializer.SeedData(context).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, "An error occurred seeding the UserData DB.");
                }
                try {
                    IdentityInitializer.SeedData(userManager, roleManager).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, "An error occurred seeding the Identity DB.");
                }
                try {
                    PromsKeyboardShortcutInitializer.SeedData(context).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, "An error occurred seeding the PromsKeyboardShortcut DB.");
                }
                try {
                    SystemParametersInitializer.SeedData(context).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, "An error occurred seeding the SystemParameters DB.");
                }
                try {
                    ProductsInitializer.SeedData(context).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, "An error occurred seeding the ProductsInitializer DB.");
                }
                try {
                    TransactionHistoryInitializer.SeedData(context).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, "An error occurred seeding the TransactionHistory DB.");
                }
                #endregion
            }

            host.Run();
            CreateWebHostBuilder(args)
                .Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(o => { o.Limits.KeepAliveTimeout = TimeSpan.FromDays(1); });
    }
}

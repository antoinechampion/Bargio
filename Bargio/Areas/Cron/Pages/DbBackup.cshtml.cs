using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

// https://www.stevejgordon.co.uk/asp-net-core-2-ihostedservice
namespace Bargio.Areas.Cron.Pages
{
    public class DbBackupModel : PageModel
    {
        private readonly IConfiguration _iConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DbBackupModel(IConfiguration iConfiguration, IHostingEnvironment hostingEnvironment) {
            _iConfiguration = iConfiguration;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty] public string Debug { get; set; }

        public IActionResult OnGet()
        {
            try {
                var connectionStringBuilder = new SqlConnectionStringBuilder {
                    ConnectionString = _iConfiguration["ConnectionStrings:DefaultConnection"]
                };
                var commandText =
                    $@"BACKUP DATABASE [{connectionStringBuilder.InitialCatalog}] TO DISK = N'{_hostingEnvironment.ContentRootPath}' WITH NOFORMAT, INIT,"
                    + "NAME = N'bargio-{DateTime.Now}', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                Debug += commandText;
                using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString)) {
                    connection.Open();
                    connection.InfoMessage += Connection_InfoMessage;
                    using (var command = connection.CreateCommand()) {
                        command.CommandText = commandText;
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e) {
                Debug = e.ToString().Replace("\n", "<br/>");
            }

            return Page();
        }

        private static void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
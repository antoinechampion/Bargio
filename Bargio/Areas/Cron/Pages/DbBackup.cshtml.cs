using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

// https://www.stevejgordon.co.uk/asp-net-core-2-ihostedservice
namespace Bargio.Areas.Cron.Pages
{
    public class DbBackupModel : PageModel
    {
        private IConfiguration _iConfiguration;

        public DbBackupModel(IConfiguration iConfiguration) {
            _iConfiguration = iConfiguration;
        }

        public void OnGet()
        {
            var connectionStringBuilder = new DbConnectionStringBuilder {
                ConnectionString = _iConfiguration["ConnectionStrings:DefaultConnection"]
            };

            var commandText = $@"BACKUP DATABASE [{connectionStringBuilder["Initial Catalog"]}] TO DISK = N'/db/' WITH NOFORMAT, INIT,'"
            + "NAME = N'bargio-{DateTime.Now}', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";

            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                connection.InfoMessage += Connection_InfoMessage;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Bargio.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bargio.Models;

namespace Bargio.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUserDefaultPwd, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Bargio.Models.UserData> UserData { get; set; }
        public DbSet<Bargio.Models.PaymentRequest> PaymentRequest { get; set; }
        public DbSet<Bargio.Models.TransactionHistory> TransactionHistory { get; set; }
        public DbSet<Bargio.Models.PromsKeyboardShortcut> PromsKeyboardShortcut { get; set; }
        public DbSet<Bargio.Models.SystemParameters> SystemParameters { get; set; }
        public DbSet<Bargio.Models.Product> Product { get; set; }
    }
}

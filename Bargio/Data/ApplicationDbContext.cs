//          Bargio - ApplicationDbContext.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Bargio.Areas.Identity;
using Bargio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUserDefaultPwd, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<UserData> UserData { get; set; }
        public DbSet<PaymentRequest> PaymentRequest { get; set; }
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<PromsKeyboardShortcut> PromsKeyboardShortcut { get; set; }
        public DbSet<SystemParameters> SystemParameters { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif
        }
    }
}
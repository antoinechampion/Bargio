using System;
using System.Collections.Generic;
using System.Text;
using Bargio.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bargio.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUserDefaultPwd, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}

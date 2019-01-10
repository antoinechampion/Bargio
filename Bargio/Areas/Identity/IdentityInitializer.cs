using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bargio.Areas.Identity
{
    public class IdentityInitializer
    {
        public static async Task SeedData
        (UserManager<IdentityUserDefaultPwd> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        public static async Task SeedUsers
            (UserManager<IdentityUserDefaultPwd> userManager)
        {
            #if DEBUG
            // PG sans mdp (hors foy'ss)
            if (userManager.FindByNameAsync
                    ("1test217").Result == null)
            {
                IdentityUserDefaultPwd user = new IdentityUserDefaultPwd
                {
                    UserName = "1test217"
                };
                IdentityResult result = await userManager.CreateAsync
                    (user, IdentityUserDefaultPwd.DefaultPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                        "PG");
                }
            }

            // Archi sans mdp
            if (userManager.FindByNameAsync
                    ("2test217").Result == null)
            {
                IdentityUserDefaultPwd user = new IdentityUserDefaultPwd
                {
                    UserName = "2test217"
                };
                IdentityResult result = await userManager.CreateAsync
                    (user, IdentityUserDefaultPwd.DefaultPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                        "Archi");
                }
            }
            #endif
            // Admin
            if (userManager.FindByNameAsync
                    ("admin").Result == null)
            {
                IdentityUserDefaultPwd user = new IdentityUserDefaultPwd
                {
                    UserName = "admin"
                };
                IdentityResult result = await userManager.CreateAsync
                    (user, "zifoys");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                        "Admin");
                }
            }
        }

        public static async Task SeedRoles
            (RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync
                ("PG"))
            {
                IdentityRole role = new IdentityRole {Name = "PG"};
                IdentityResult roleResult = await roleManager.
                    CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync
                ("Archi"))
            {
                IdentityRole role = new IdentityRole { Name = "Archi" };
                IdentityResult roleResult = await roleManager.
                    CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync
                ("Admin"))
            {
                IdentityRole role = new IdentityRole { Name = "Admin" };
                IdentityResult roleResult = await roleManager.
                    CreateAsync(role);
            }
        }
    }
}

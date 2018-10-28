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
            // PG sans mdp (hors foy'ss)
            if (userManager.FindByNameAsync
                    ("1Test217").Result == null)
            {
                IdentityUserDefaultPwd user = new IdentityUserDefaultPwd
                {
                    UserName = "1Test217"
                };
                IdentityResult result = await userManager.CreateAsync
                    (user, IdentityUserDefaultPwd.DefaultPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                        "PG");
                }
            }

            // PG avec mdp
            if (userManager.FindByNameAsync
                    ("2Test217").Result == null)
            {
                IdentityUserDefaultPwd user = new IdentityUserDefaultPwd
                {
                    UserName = "2Test217"
                };
                IdentityResult result = await userManager.CreateAsync
                    (user, "testmdp");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                        "PG");
                }
            }

            // Archi sans mdp
            if (userManager.FindByNameAsync
                    ("3Test217").Result == null)
            {
                IdentityUserDefaultPwd user = new IdentityUserDefaultPwd
                {
                    UserName = "3Test217"
                };
                IdentityResult result = await userManager.CreateAsync
                    (user, IdentityUserDefaultPwd.DefaultPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                        "Archi");
                }
            }

            // ZiFoy's
            if (userManager.FindByNameAsync
                    ("4Test217").Result == null)
            {
                IdentityUserDefaultPwd user = new IdentityUserDefaultPwd
                {
                    UserName = "4Test217"
                };
                IdentityResult result = await userManager.CreateAsync
                    (user, IdentityUserDefaultPwd.DefaultPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                        "ZiFoys");
                }
            }

            // Interface babasse
            if (userManager.FindByNameAsync
                    ("Babasse").Result == null)
            {
                IdentityUserDefaultPwd user = new IdentityUserDefaultPwd
                {
                    UserName = "Babasse"
                };
                IdentityResult result = await userManager.CreateAsync
                    (user, IdentityUserDefaultPwd.DefaultPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                        "Babasse");
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
                ("ZiFoys"))
            {
                IdentityRole role = new IdentityRole { Name = "ZiFoys" };
                IdentityResult roleResult = await roleManager.
                    CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync
                ("Babasse"))
            {
                IdentityRole role = new IdentityRole { Name = "Babasse" };
                IdentityResult roleResult = await roleManager.
                    CreateAsync(role);
            }
        }
    }
}

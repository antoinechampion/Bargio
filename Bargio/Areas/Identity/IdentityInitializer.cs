//          Bargio - IdentityInitializer.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bargio.Areas.Identity
{
    public class IdentityInitializer
    {
        public static async Task SeedData
        (UserManager<IdentityUserDefaultPwd> userManager,
            RoleManager<IdentityRole> roleManager) {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        public static async Task SeedUsers
            (UserManager<IdentityUserDefaultPwd> userManager) {
#if DEBUG
            // PG sans mdp (hors foy'ss)
            if (userManager.FindByNameAsync
                    ("1test217").Result == null) {
                var user = new IdentityUserDefaultPwd {
                    UserName = "1test217"
                };
                var result = await userManager.CreateAsync
                    (user, IdentityUserDefaultPwd.DefaultPassword);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user,
                        "PG");
            }

            // Archi sans mdp
            if (userManager.FindByNameAsync
                    ("2test217").Result == null) {
                var user = new IdentityUserDefaultPwd {
                    UserName = "2test217"
                };
                var result = await userManager.CreateAsync
                    (user, IdentityUserDefaultPwd.DefaultPassword);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user,
                        "Archi");
            }
#endif
            // Admin
            if (userManager.FindByNameAsync
                    ("admin").Result == null) {
                var user = new IdentityUserDefaultPwd {
                    UserName = "admin"
                };
                var result = await userManager.CreateAsync
                    (user, "zifoys");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user,
                        "Admin");
            }

            // Babasse
            if (userManager.FindByNameAsync
                    ("babasse").Result == null) {
                var user = new IdentityUserDefaultPwd {
                    UserName = "babasse"
                };
                var result = await userManager.CreateAsync
                    (user, "zifoys");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user,
                        "Babasse");
            }
        }

        public static async Task SeedRoles
            (RoleManager<IdentityRole> roleManager) {
            if (!await roleManager.RoleExistsAsync
                ("PG")) {
                var role = new IdentityRole {Name = "PG"};
                var roleResult = await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync
                ("Archi")) {
                var role = new IdentityRole {Name = "Archi"};
                var roleResult = await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync
                ("Admin")) {
                var role = new IdentityRole {Name = "Admin"};
                var roleResult = await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync
                ("Babasse")) {
                var role = new IdentityRole {Name = "Babasse"};
                var roleResult = await roleManager.CreateAsync(role);
            }
        }
    }
}
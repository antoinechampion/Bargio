//          Bargio - Startup.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.Linq;
using Bargio.Areas.Identity;
using Bargio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bargio
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(options => {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 0;
                options.Password.RequiredUniqueChars = 1;
            });


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUserDefaultPwd, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options => {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
            });
            services.AddAuthorization(options => {
                options.AddPolicy("RequireBabasseRole", policy => policy.RequireRole("Babasse"));
            });

            services.AddHttpClient();

            services
                .AddMvc(config => {
                    // using Microsoft.AspNetCore.Mvc.Authorization;
                    // using Microsoft.AspNetCore.Authorization;
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options => {
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/Logout", "logout");
                    options.Conventions.AddAreaPageRoute("User", "/Dashboard", "pg");
                    options.Conventions.AddAreaPageRoute("User", "/Babasse", "babasse");
                    options.Conventions.AddPageRoute("/Error", "Error/{statusCode}");
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Logout");
                    options.Conventions.AuthorizeAreaFolder("User", "/");
                    options.Conventions.AuthorizeAreaFolder("Admin", "/", "RequireAdministratorRole");
                    options.Conventions.AuthorizeAreaFolder("Cron", "/", "RequireAdministratorRole");
                    options.Conventions.AuthorizeAreaPage("User", "/Babasse", "RequireBabasseRole");
                });

            services.ConfigureApplicationCookie(options => {
                options.ExpireTimeSpan = TimeSpan.FromDays(300);
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Error/403";
            });

            services.Configure<SecurityStampValidatorOptions>(options =>
            options.ValidationInterval = TimeSpan.FromDays(300));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            UserManager<IdentityUserDefaultPwd> userManager,
            RoleManager<IdentityRole> roleManager) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else {
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Middleware to redirect in case of maintainance
            app.Use(async (httpCtx, next) => {
                var context = httpCtx.RequestServices.GetRequiredService<ApplicationDbContext>();

                if (context.SystemParameters.First().Maintenance
                    && !httpCtx.Request.GetEncodedUrl().Contains("Error")) {
                    httpCtx.Response.Redirect("/Error/503");
                    return;
                }
                await next();
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
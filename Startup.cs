using MangaCMS.DAL;
using MangaCMS.Models;
using MangaCMS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS
{
    public class Startup
    {
        // свойство, которое будет хранить конфигурацию
        public IConfiguration AppConfiguration { get;}

        public Startup()
        {
            var builder =
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables();

            AppConfiguration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MangaCMSContext>(options => options.UseNpgsql(AppConfiguration.GetConnectionString("MangaCMSDatabase")));


            services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<MangaCMSContext>();

            var test = AppConfiguration["Test:SyncKey"];

            var optionsBuilder = new DbContextOptionsBuilder<MangaCMSContext>();
            optionsBuilder.UseNpgsql(AppConfiguration.GetConnectionString("MangaCMSDatabase"));

            


            //using (MangaCMSContext db = new MangaCMSContext(optionsBuilder.Options))
            //{
            //    CustomUser u1 = new CustomUser { UserName = "Test", PasswordHash = "qwer" };

            //    db.Add(u1);
            //    db.SaveChanges();


            //    var Users = db.Users.ToList();
            //}

        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<CustomUser>>();

            string[] roleList = { "Admin", "User", "Moder", "Cleaner", "Translator", "Corrector", "Typer", "Editor" };

            IdentityResult roleResult;

            foreach (var roleName in roleList)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the DB
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Find superuser in DB
            var _superuser = await UserManager.FindByEmailAsync("Admin@Admin");

            if (_superuser == null)
            {

                //var poweruser = new IdentityUser {

                //    UserName = Configuration["AppSettings:AdminName"],
                //    Email = Configuration["AppSettings:AdminEmail"],
                //};
                //string userPWD = Configuration["AppSettings:AdminPassword"];
                var poweruser = new CustomUser
                {
                    
                    UserName = "Admin",
                    Email = "Admin@Admin",
                    EmailConfirmed = true,
                };
                string userPWD = "_Admin123";

                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {

                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("HELLO");
                });
            });
            var serviceProvider = app.ApplicationServices.GetService<IServiceProvider>();
            CreateRoles(serviceProvider).Wait();


        }
    }
}

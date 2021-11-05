using MangaCMS.DAL;
using MangaCMS.Models;
using MangaCMS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public IConfiguration AppConfiguration { get; set; }

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
            //EnvironmentVerifier EV = new();
            //EV.Checking();
            var test = AppConfiguration["Test:SyncKey"];

            var optionsBuilder = new DbContextOptionsBuilder<MangaCMSContext>();
            optionsBuilder.UseNpgsql(AppConfiguration.GetConnectionString("MangaCMSDatabase"));



            using (MangaCMSContext db = new MangaCMSContext(optionsBuilder.Options))
            {
                CustomUser u1 = new CustomUser { UserName = "Test", PasswordHash = "qwer" };

                db.Add(u1);
                db.SaveChanges();


                var Users = db.Users.ToList();
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
        }
    }
}

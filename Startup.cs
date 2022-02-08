using MangaCMS.DAL;
using MangaCMS.Models;
using MangaCMS.Services;
using MangaCMS.Services.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MangaCMS
{
    public class Startup
    {
        // свойство, которое будет хранить конфигурацию
        public IConfiguration Configuration { get;}

        public Startup()
        {
            var builder =
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var signingKey = new SigningSymmetricKey(Configuration["Test:SymmetricKey"]);
            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);



            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            ); ;

            const string jwtSchemeName = "JwtBearer";
            var signingDecodingKey = (IJwtSigningDecodingKey)signingKey;
            services
                .AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = jwtSchemeName;
                    options.DefaultChallengeScheme = jwtSchemeName;
                })
                .AddJwtBearer(jwtSchemeName, jwtBearerOptions => {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingDecodingKey.GetKey(),

                        ValidateIssuer = true,
                        ValidIssuer = SigningSymmetricKey.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = SigningSymmetricKey.AUDIENCE,

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5),
                        
                    };
                    jwtBearerOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthentication();

           // services.AddDbContext<MangaCMSContext>(options => options.UseNpgsql(Configuration.GetConnectionString("MangaRemoteDBPG")));
            services.AddDbContext<MangaCMSContext>(options => options.UseNpgsql(Configuration.GetConnectionString("MangaCMSDBPG")));


            services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<MangaCMSContext>();

            //var test = Configuration["Test:SymmetricKey"];

            services.AddCors();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MangaCMS API"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Description = "Please insert JWT token into field"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    {
                         new OpenApiSecurityScheme
                         {
                               Reference = new OpenApiReference
                               {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                               }
                         },
                         new string[] { }
                    }
                });

                //Set the comments path for the swagger json and ui.
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            services.AddApiVersioning();

            //var optionsBuilder = new DbContextOptionsBuilder<MangaCMSContext>();
            //optionsBuilder.UseNpgsql(Configuration.GetConnectionString("MangaCMSDBPG"));
            //using (MangaCMSContext db = new MangaCMSContext(optionsBuilder.Options))
            //{
            //    CustomUser u1 = new CustomUser { UserName = "Test", PasswordHash = "qwer" };

            //    db.Add(u1);
            //    db.SaveChanges();


            //    var Users = db.Users.ToList();
            //}

        }

            private async Task CreateRoles(RoleManager<IdentityRole> RoleManager, UserManager<CustomUser> UserManager)
        {

            //var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //var UserManager = serviceProvider.GetRequiredService<UserManager<CustomUser>>();

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
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            RoleManager<IdentityRole> RoleManager, 
            UserManager<CustomUser> UserManager,
            MangaCMSContext mangaCMSContext
            )
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MangaCMSl API V1");
            });
            app.UseApiVersioning();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            //var serviceProvider = app.ApplicationServices.GetService<IServiceProvider>();
            //var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //var UserManager = serviceProvider.GetRequiredService<UserManager<CustomUser>>();

            mangaCMSContext.Database.Migrate();

            CreateRoles(RoleManager, UserManager).Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

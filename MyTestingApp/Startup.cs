using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyTestingApp.Models;
using MyTestingApp.Services.Interfaces;
using MyTestingApp.Services.Repositories;
using MyTestingApp.Utilities;

namespace MyTestingApp
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            // var appSettings = Configuration.GetSection(AppSetting.Settings);
            // services.Configure<AppSetting>(appSettings);
            // var settings = appSettings.Get<AppSetting>();
            #region JwtAuthentication
            var jwtSection = Configuration.GetSection("JwtBearerTokenSettings");
            services.Configure<AppSetting>(jwtSection);
            var jwtBearerTokenSettings = jwtSection.Get<AppSetting>();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.Key);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                     .AddJwtBearer(options =>
                     {
                         options.RequireHttpsMetadata = false;
                         options.SaveToken = true;
                         options.TokenValidationParameters = new TokenValidationParameters()
                         {
                             ValidateIssuer = true,
                             ValidIssuer = jwtBearerTokenSettings.Issuer,
                             ValidateAudience = true,
                             ValidAudience = jwtBearerTokenSettings.Issuer,
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = new SymmetricSecurityKey(key),
                             ValidateLifetime = true,
                             ClockSkew = TimeSpan.Zero
                         };
                     });
            #endregion
            services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("PostgresSqlConnection"));
                });
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<ICategory, CategoryRepository>();
            services.AddScoped<IProduct, ProductRespository>();
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // using (var serviceScope = app.ApplicationServices
            // .GetRequiredService<IServiceScopeFactory>()
            // .CreateScope())
            //     {
            //         using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
            //         {
            //             context.Database.Migrate();
            //         }
            //     }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

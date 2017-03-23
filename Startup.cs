using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dsa.RapidResponse.Services;
using Dsa.RapidResponse.Implementations;

namespace Dsa.RapidResponse
{
    public class Startup
    {
        // this was useful https://blogs.msdn.microsoft.com/cjaliaga/2016/08/10/working-with-azure-app-services-application-settings-and-connection-strings-in-asp-net-core/ 
        // this seems like an unneccesary hack, but it works
        private bool _isDevelopment;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _isDevelopment = env.IsDevelopment();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            
            if(_isDevelopment)
            {
                services.AddDbContext<ComradeDbContext>(options =>
                    options.UseSqlite("Data Source=comrades.sqlite",
                        optionsBuilder => optionsBuilder.MigrationsAssembly("Dsa.RapidResponse")));
            }
            else
            {
                var connectionString = Configuration.GetConnectionString("defaultConnection");
                services.AddDbContext<ComradeDbContext>(options => options.UseSqlServer(connectionString));
                //services.AddDbContext<ComradeDbContext>(options =>
                //    options.UseSqlServer(connectionString,
                //        optionsBuilder => optionsBuilder.MigrationsAssembly("Dsa.RapidResponse")));
            }
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ComradeDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IMessagingService, SmsService>();
            services.AddTransient<IEmailService, EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ComradeDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            /*}
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }*/

            app.UseStaticFiles();
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInit.Init(dbContext, userManager);
        }
    }

    public class DbInit
    {
        public static void Init(ComradeDbContext context, UserManager<IdentityUser> userManager)
        {
            context.Database.Migrate();
            var u = userManager.FindByEmailAsync("thebeekeeper@gmail.com").Result;
            if(u == null)
            {
                System.Diagnostics.Debug.WriteLine("no user");
            }
            else
            {
                var r = userManager.AddClaimAsync(u, new Claim(ClaimTypes.Role, "Administrator")).Result;
                System.Diagnostics.Debug.WriteLine(r.Succeeded);
            }
        }
    }
}

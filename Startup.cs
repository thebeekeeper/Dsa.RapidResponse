using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Dsa.RapidResponse.Services;
using Dsa.RapidResponse.Implementations;

namespace Dsa.RapidResponse
{
    public class Startup
    {
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
                services.AddDbContext<ComradeDbContext>(options =>
                    options.UseSqlServer(connectionString,
                        optionsBuilder => optionsBuilder.MigrationsAssembly("Dsa.RapidResponse")));
            }
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ComradeDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IMessagingService, SmsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ComradeDbContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInit.Init(dbContext);
        }
    }

    public class DbInit
    {
        public static void Init(ComradeDbContext context)
        {
            context.Database.EnsureCreated();

            var u = context.Users.FirstOrDefault();
            if(u == null)
            {
                System.Diagnostics.Debug.WriteLine("no user");
            }
            else
            {
                //u.PhoneNumber = "3126361051";
            }
            context.SaveChanges();
        }
    }
}

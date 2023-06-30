using GProject.WebApplication.Services.IServices;
using GProject.WebApplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using GProject.Data.DomainClass;
using GProject.Data.Context;
using GProject.WebApplication.Services;
using System.Configuration;
using RabbitMQ.Client;
using System.Net.Mail;
using System.Net;
using System.Web.Http;
using GProject.WebApplication.Helpers;
using static IdentityModel.ClaimComparer;

namespace YourNamespace
{
    public class Program
    {
        private static QuartzService _quartzService;

        public static void Main(string[] args)
        {
            _quartzService = new QuartzService();
            _quartzService.Start();
            var host = CreateHostBuilder(args).Build();
            host.Run();           
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(c =>
            {
                c.IOTimeout = TimeSpan.FromSeconds(20);
                c.Cookie.HttpOnly = true;
                c.Cookie.IsEssential = true;
            });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();

            services.AddDbContext<GProjectContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(dd =>
            {
                dd.LoginPath = "/Account/login";
                dd.LogoutPath = "/Account/logout";
                dd.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddGoogle(yy =>
            {
                yy.ClientId = "157635440787-v046f76b5rm5h0dfreqic0ams7cn0qj1.apps.googleusercontent.com";
                yy.ClientSecret = "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";
                yy.SignInScheme = IdentityConstants.ExternalScheme;
				yy.Events.OnRedirectToAuthorizationEndpoint = context =>
				{
					context.Response.Redirect(context.RedirectUri + "&prompt=consent");
					return Task.CompletedTask;
				};
			});
			services.AddRazorPages()
	.AddRazorRuntimeCompilation();
			services.AddScoped<IVnPayService, VnPayService>();
			services.AddAuthorization();
            services.AddIdentity<Customer, IdentityRole>()
                .AddEntityFrameworkStores<GProjectContext>()
                .AddDefaultTokenProviders();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

			app.ApplicationServices
				.CreateScope()
				.ServiceProvider
				.GetRequiredService<IWebHostEnvironment>();

			app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");
            });
        }
    }
    public class RabbitMQOptions
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
    }
}

#region Old
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.Google;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Options;
//using GProject.Data.DomainClass;
//using GProject.Data.Context;
//using GProject.WebApplication.Services;


//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSession(c =>
//{
//    c.IOTimeout = TimeSpan.FromDays(1);
//}
//);
//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddSession(c =>
//{
//    c.IOTimeout = TimeSpan.FromSeconds(20);
//    c.Cookie.HttpOnly = true;
//    c.Cookie.IsEssential = true;
//}
//            );
//builder.Services.AddDbContext<GProjectContext>();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//        .AddCookie(dd =>
//        {
//            dd.LoginPath = "/Account/login";
//            dd.LogoutPath = "/Account/logout";
//            dd.AccessDeniedPath = "/Account/AccessDenied";
//        })
//        .AddGoogle(yy =>
//        {
//            yy.ClientId = "157635440787-v046f76b5rm5h0dfreqic0ams7cn0qj1.apps.googleusercontent.com";
//            yy.ClientSecret = "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";
//        });

//builder.Services.AddIdentity<Customer, IdentityRole>()
//    .AddEntityFrameworkStores<GProjectContext>()
//    .AddDefaultTokenProviders();

//builder.Services.AddScoped<IOAuthRepository, OAuthRepository>();
//builder.Services.AddAuthorization();
//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseAuthentication();
//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseSession();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}/{id?}");

//app.Run();

#endregion

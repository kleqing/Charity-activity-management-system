using BussinessObject;
using DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Repository;

namespace CharityActivityWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Cookie
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            //{
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            //    options.SlidingExpiration = true;
            //    options.AccessDeniedPath = "/Forbidden/";
            //    options.LoginPath = "/Admin/Login/Index";
            //    options.ReturnUrlParameter = "returnUrl";
            //}).AddCookie("Client", options =>
            //{
            //    options.LoginPath = new PathString("/Client/Home");
            //});

            // Add services to the container.
			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddScoped<DBContext>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserDAO>();

			builder.Services.AddControllersWithViews();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllerRoute(
				name: "areas",
				pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

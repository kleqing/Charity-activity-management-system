﻿using Dynamics.DataAccess;
using Dynamics.DataAccess.Repository;
using Dynamics.Services;
using Dynamics.Utility;
using Dynamics.Utility.Mapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Cloudinary = CloudinaryDotNet.Cloudinary;


namespace Dynamics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            
            // Add cache to the container, allow admin dashboard get the latest data
            // working with other services as well
            builder.Services.AddMemoryCache();

            // Add cache to the container, allow admin dashboard get the latest data
            // working with other services as well
            builder.Services.AddMemoryCache();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Add service and scope for Google auth
            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                googleOptions.Scope.Add(Google.Apis.Oauth2.v2.Oauth2Service.Scope.UserinfoProfile);
                googleOptions.Scope.Add(Google.Apis.Oauth2.v2.Oauth2Service.Scope.UserinfoEmail);

                // Get user profile
                googleOptions.ClaimActions.MapJsonKey("picture", "picture");
            });
            // Add database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
                // options.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
                // options.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
            });
            builder.Services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDbContextConnection"));
            });

            // Identity and roles
            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedAccount = true; // No confirm account required
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            // Configure authentication cookie
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.SlidingExpiration = true; // Auto re-new authentication cookie when almost ended
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Cookie expires after 60 minutes
                });

            // Add authorization policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(RoleConstants.Admin));
            });



            // Add authorization policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(RoleConstants.Admin));
            });


            // Repos here
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<IOrganizationVMService, OrganizationVMService>();
            builder.Services.AddScoped<IUserToOragnizationTransactionHistoryVMService, UserToOragnizationTransactionHistoryVMService>();
            builder.Services.AddScoped<IProjectVMService, ProjectVMService>();
            builder.Services.AddScoped<IOrganizationToProjectHistoryVMService, OrganizationToProjectHistoryVMService>();

            builder.Services.AddScoped<IRequestRepository, RequestRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            // Project repos
            
            builder.Services.AddScoped<IProjectResourceRepository, ProjectResourceRepository>();
            builder.Services.AddScoped<IProjectHistoryRepository, ProjectHistoryRepository>();
            builder.Services.AddScoped<IOrganizationToProjectTransactionHistoryRepository, OrganizationToProjectTransactionHistoryRepository>();
            builder.Services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
            builder.Services.AddScoped<IUserToProjectTransactionHistoryRepository,UserToProjectTransactionHistoryRepository>();
            // Organization repos
            builder.Services.AddScoped<IOrganizationMemberRepository, OrganizationMemberRepository>();
            builder.Services.AddScoped<IOrganizationResourceRepository, OrganizationResourceRepository>();
            builder.Services
                .AddScoped<IUserToOrganizationTransactionHistoryRepository,
                    UserToOrganizationTransactionHistoryRepository>();
            // Automapper
            builder.Services.AddAutoMapper(typeof(MyMapper));
            // Add custom services
            builder.Services.AddScoped<ITransactionViewService, TransactionViewService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddScoped<IOrganizationService, OrganizationService>();
            // Add email sender
            builder.Services.AddScoped<IEmailSender, EmailSender>();

            // Cloudinary
            builder.Services.AddSingleton<CloudinaryUploader>();
            
            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            // Configure default routes (This should be after configured the Identity)
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            // Enable razor page
            builder.Services.AddRazorPages();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            });

            // Add serilog for debugging
            var logger = new LoggerConfiguration()
               .WriteTo.Console()
               // .WriteTo.File("Logs/Logs.txt", rollingInterval: RollingInterval.Minute)
               .MinimumLevel.Information() // You can change this one so that it filters out stuff
               .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger); // This one make it so that ASP will use it

            var app = builder.Build();
            // Redirect user to 404 page if not found
            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/error/PageNotFound";
                    await next();
                }
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.MapControllers();
            app.UseRouting();
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            
            app.MapControllerRoute(
                 name: "areas",
                 pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Homepage}/{id?}");

            app.Run();
        }
    }
}
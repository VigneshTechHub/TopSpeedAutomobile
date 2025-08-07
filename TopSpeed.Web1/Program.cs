using Microsoft.EntityFrameworkCore;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Infrastructure.Common;
using TopSpeed.Infrastructure.Repositories;
using TopSpeed.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TopSpeed.Application.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using TopSpeed.Application.Services.Interface;
using Serilog;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        object value = builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer
        (builder.Configuration.GetConnectionString("DefaultConnetion")));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = $"/Identity/Account/Login";
            options.LogoutPath = $"/Identity/Account/Logout";
            options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

        });

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
        });

        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IEmailSender, EmailSender>();
        builder.Services.AddScoped<IUserNameService, UserNameService>();
        builder.Services.AddHttpContextAccessor();
        //builder.Services.AddScoped<IBrandRepository, BrandRepository>();

        #region Configuration For Seeding Data To DataBase


        static async void UpdateDatabaseAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var Services = scope.ServiceProvider;

                try
                {
                    var context = Services.GetRequiredService<ApplicationDbContext>();

                    if (context.Database.IsSqlServer())
                    {
                        context.Database.Migrate();
                    }
                    await SeedData.SeedDataAsync(context);
                }

                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occured while migrating or seeding the data");

                }

            }
        }
        #endregion

        builder.Host.UseSerilog((context, config) =>
        {
            config.WriteTo.File("Log/Log.txt", rollingInterval: RollingInterval.Day);

            if (context.HostingEnvironment.IsProduction() == false)
            {
                config.WriteTo.Console();
            }
        });

        builder.Services.AddControllersWithViews();

        builder.Services.AddRazorPages();

        var app = builder.Build();

        var ServiceProvider = app.Services;
        await SeedData.SeedRole(ServiceProvider);

        UpdateDatabaseAsync(app);

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

        app.UseAuthorization();

        app.UseAuthentication();

        app.UseSession();

        app.MapRazorPages();

        app.MapControllerRoute(
            name: "default",
            pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}

#region Configuration For Seeding Data To DataBase

#endregion

using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YemeniDriver.Interfaces;
using YemeniDriver.Repository;
using YemeniDriver.Service;
using YemeniDriver.Data;
using YemeniDriver.Helpers;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.Repository;
using YemeniDriver.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IDriverAndRequestRepository, DriverAndRequestRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseSettings")),
    ServiceLifetime.Scoped);



builder.Services.AddIdentity<ApplicationUser , IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
var app = builder.Build();


if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);

    //Seed.SeedData(app);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/notificationHub");
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();

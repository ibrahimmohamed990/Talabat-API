using AdminDashboard.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data.Context;
using Store.Data.Entities.Identity_Entities;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<StoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Scoped);

builder.Services.AddDbContext<StoreIdentityDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
}, ServiceLifetime.Scoped);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Admin/Login";
            options.LogoutPath = "/Admin/Logout";
            options.AccessDeniedPath = "/Admin/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromDays(1); // Set the cookie expiration time to 1 day
            options.SlidingExpiration = true;
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccessDenied", policy =>
    {
        policy.RequireAuthenticatedUser().RequireRole("Admin"); // Example policy, adjust as needed
    });
});


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

}).AddEntityFrameworkStores<StoreIdentityDBContext>();
//builder.Services.AddScoped<UserManager<AppUser>>();
//builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

builder.Services.AddAutoMapper(typeof(MapsProfile));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithRedirects("/Admin/AccessDenied?statusCode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Admin}/{action=Login}");

app.Run();

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using webapp.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<RestaurantContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
 .AddRoles<IdentityRole>()
 .AddEntityFrameworkStores<RestaurantContext>()
 .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(OptionsBuilderConfigurationExtensions =>
{
    OptionsBuilderConfigurationExtensions.LoginPath = new PathString("/Account/Login");
    OptionsBuilderConfigurationExtensions.AccessDeniedPath = new PathString("/Account/AccessDenied");
    OptionsBuilderConfigurationExtensions.LogoutPath = new PathString("/Index");
});

builder.Services.AddControllersWithViews();

#region Authorization

    AddAuthorizationPolicies(builder.Services);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

void AddAuthorizationPolicies(IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
        options.AddPolicy("User", policy => policy.RequireClaim("SignedIn"));
    });
    services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    });
}

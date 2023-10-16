using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//1. add the authentication service
//2. Authentication scheme
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "SmartCookies";
        options.DefaultChallengeScheme = "SmartCookies";
    })
    .AddCookie("SmartCookies",options =>
    {
        options.LoginPath = "/Home/LoginPost";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = true;
        options.Events = new CookieAuthenticationEvents()
        {
            OnValidatePrincipal = (prinicple) =>
            {
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("AdminOnly",builder=>builder.RequireClaim("Admin"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

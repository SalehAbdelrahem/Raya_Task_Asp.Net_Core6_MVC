using BLL.Services.HRs;
using BLL.Services.Roles;
using DAL.Contacts.HRS;
using DAL.Data;
using DAL.Models;
using DAL.Repositories.HRS;
using DMSTaskMVC.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDBContext>(options =>
{
    //options.UseLazyLoadingProxies();
    options.UseSqlServer(builder.Configuration.GetConnectionString("Dbconnectionstring"));

});
builder.Services.AddIdentity<User, Role>(options =>
{
    options.User.RequireUniqueEmail = true;
    //options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.Name = "YourAppCookieName";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.LoginPath = "/Accounts/LogIN";
    // ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews();

//object Add scope
builder.Services.AddScoped<ServiceHR>();
builder.Services.AddScoped<ServiceRole>();
builder.Services.AddScoped<IHrRepository, HrRepository>();


//congiguration AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


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

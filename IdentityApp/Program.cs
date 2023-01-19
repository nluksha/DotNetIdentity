using Microsoft.EntityFrameworkCore;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using IdentityApp.Services;
using IdentityApp;

// Configure services
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDataConnection");
var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");

builder.Services.AddHttpsRedirection(opts =>
{
  opts.HttpsPort = 44350;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<IdentityDbContext>(options =>
  options.UseSqlServer(
    identityConnectionString,
    opts => opts.MigrationsAssembly("IdentityApp")
  )
);
builder.Services.AddScoped<IEmailSender, ConsoleEmailSender>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
  opts.Password.RequiredLength = 8;
  opts.Password.RequireDigit = false;
  opts.Password.RequireLowercase = false;
  opts.Password.RequireUppercase = false;
  opts.Password.RequireNonAlphanumeric = false;
  opts.SignIn.RequireConfirmedAccount = true;
}).AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<SecurityStampValidatorOptions>(opts =>
{
  opts.ValidationInterval = System.TimeSpan.FromMinutes(1);
});

builder.Services.AddScoped<TokenUrlEncoderService>();
builder.Services.AddScoped<IdentityEmailService>();

builder.Services.AddAuthentication()
  .AddFacebook(opts =>
  {
    opts.AppId = builder.Configuration["Facebook:AppId"];
    opts.AppSecret = builder.Configuration["Facebook:AppSecret"];
  })
  .AddGoogle(opts =>
  {
    opts.ClientId = builder.Configuration["Google:ClientId"];
    opts.ClientSecret = builder.Configuration["Google:ClientSecret"];
  });

builder.Services.ConfigureApplicationCookie(opts =>
{
  opts.LoginPath = "/Identity/SignIn";
  opts.LogoutPath = "/Identity/SignOut";
  opts.AccessDeniedPath = "/Identity/Forbidden";
  opts.Events.DisableRedirectionForApiClients();
});

// Configure 
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
  endpoints.MapDefaultControllerRoute();
  endpoints.MapRazorPages();
});

app.SeedUserStoreForDashboard();

app.Run();

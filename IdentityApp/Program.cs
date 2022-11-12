using Microsoft.EntityFrameworkCore;
using IdentityApp.Models;

// Configure services
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDataConnection");

builder.Services.AddHttpsRedirection(opts => {
  opts.HttpsPort = 44350;
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(connectionString));

// Configure
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints => {
  endpoints.MapDefaultControllerRoute();
  endpoints.MapRazorPages();
});

app.Run();

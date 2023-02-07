
// Configure services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure 
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
  endpoints.MapGet("/", () => "Hello World!");
  endpoints.MapRazorPages();
  endpoints.MapDefaultControllerRoute();
});

app.Run();

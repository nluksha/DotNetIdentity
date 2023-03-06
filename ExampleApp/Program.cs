using ExampleApp;
using ExampleApp.Custom;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

// Configure services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IAuthorizationHandler, CustomRequirementHandler>();

builder.Services
    .AddAuthentication(opts =>
    {
        opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(opts =>
    {
        opts.LoginPath = "/signin";
        opts.AccessDeniedPath = "/signin/403";
    });
builder.Services.AddAuthorization(opts =>
{
    AuthorizationPolicies.AddPolicies(opts);
});
builder.Services.AddRazorPages(opts =>
{
    opts.Conventions.AuthorizePage("/Secret", "NotAdmins");
});
builder.Services.AddControllersWithViews(opts =>
{
    opts.Conventions.Add(new AuthorizationPolicyConvention("Home", policy: "NotAdmins"));
    opts.Conventions.Add(
        new AuthorizationPolicyConvention("Home", action: "Protected", policy: "UsersExceptBob")
    );
});

// Configure
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseAuthentication();

// app.UseMiddleware<RoleMemberships>();
app.UseRouting();

// app.UseMiddleware<ClaimsReporter>();
// app.UseMiddleware<CustomAuthorization>();
// app.UseAuthorization();
app.UseMiddleware<AuthorizationReporter>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", () => "Hello World!");
    // endpoints.MapGet("/secret", SecretEndpoint.Endpoint).WithDisplayName("secret");
    // endpoints.MapGet("/signin", CustomSignInAndSignOut.SignIn);
    // endpoints.MapGet("/signout", CustomSignInAndSignOut.SignOut);

    endpoints.MapRazorPages();
    endpoints.MapDefaultControllerRoute();
});

app.Run();

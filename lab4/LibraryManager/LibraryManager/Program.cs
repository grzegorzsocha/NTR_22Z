using LibraryManager.Core;
using LibraryManager.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("libraryDatabase")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
      {
          options.Cookie.Name = "UserLoginCookie";
          options.SlidingExpiration = true;
          options.ExpireTimeSpan = new TimeSpan(1, 0, 0); // Expires in 1 hour
          options.Events.OnRedirectToLogin = (context) =>
          {
              context.Response.StatusCode = StatusCodes.Status401Unauthorized;
              return Task.CompletedTask;
          };

          options.Cookie.HttpOnly = true;
          // Only use this when the sites are on different domains
          options.Cookie.SameSite = SameSiteMode.None;
      });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.AdminOnly, policy => policy.RequireClaim(ClaimTypes.Role, Roles.Admin));
    options.AddPolicy(Policies.UserOnly, policy => policy.RequireClaim(ClaimTypes.Role, Roles.User));
});

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()
           .SetIsOriginAllowed(origin =>
           {
               if (string.IsNullOrWhiteSpace(origin)) return false;
               // Only add this to allow testing with localhost, remove this line in production!
               if (origin.ToLower().StartsWith("http://localhost")) return true;
               // Insert your production domain here.
               if (origin.ToLower().StartsWith("https://dev.mydomain.com")) return true;
               return false;
           });
}));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html"); ;

app.Run();

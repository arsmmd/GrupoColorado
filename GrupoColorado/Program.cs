using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace GrupoColorado
{
  public class Program
  {
    public static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services
        .AddControllersWithViews()
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
          options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
          options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

      builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          options.LoginPath = "/Account/Login";
          options.LogoutPath = "/Account/Logout";
          options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
          options.AccessDeniedPath = "/Auth/AccessDenied";
        });

      builder.Services.AddHttpContextAccessor();

      builder.Services.AddHttpClient("Api", client =>
      {
        client.BaseAddress = new Uri(builder.Configuration["API:BaseAddress"]);
      });

      WebApplication app = builder.Build();

      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}");

      app.Run();
    }
  }
}
using GrupoColorado.DTOs.Core;
using GrupoColorado.Extensions;
using GrupoColorado.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrupoColorado.Controllers
{
  public class AccountController : Controller
  {
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
      _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
      Response.Cookies.Delete("access_token");
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
        return View(model);

      StringContent content = new(model.Serialize(), Encoding.UTF8, "application/json");

      HttpClient apiClient = _httpClientFactory.CreateClient("Api");
      HttpResponseMessage response = await apiClient.PostAsync("auth/login", content);
      if (!response.IsSuccessStatusCode)
      {
        ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
        return View(model);
      }

      string responseContent = await response.Content.ReadAsStringAsync();
      AuthResponse authResult = responseContent.Deserialize<AuthResponse>();

      // Armazena o token no cookie
      Response.Cookies.Append("access_token", authResult.Token, new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        Expires = DateTimeOffset.UtcNow.AddHours(1)
      });

      List<Claim> claims = new()
      {
        new Claim(ClaimTypes.NameIdentifier, authResult.CodigoUsuario.ToString()),
        new Claim(ClaimTypes.Name, authResult.Nome),
        new Claim(ClaimTypes.Email, authResult.Email)
      };

      ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new(identity));

      return RedirectToAction("Index", "Home");
    }
  }
}
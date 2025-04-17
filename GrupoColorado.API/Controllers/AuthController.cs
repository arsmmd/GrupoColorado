using GrupoColorado.API.DTOs;
using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GrupoColorado.API.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    private readonly IUsuarioService _usuarioService;

    public AuthController(IConfiguration configuration, IUsuarioService usuarioService)
    {
      _configuration = configuration;
      _usuarioService = usuarioService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
      Usuario usuario = await _usuarioService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);

      if (usuario == null)
        return Unauthorized();

      return Ok(new
      {
        codigoUsuario = usuario.CodigoUsuario,
        nome = usuario.Nome,
        email = usuario.Email,
        token = this.GenerateToken(usuario)
      });
    }

    private string GenerateToken(Usuario usuario)
    {
      byte[] key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

      Claim[] claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, usuario.CodigoUsuario.ToString()),
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim(ClaimTypes.Email, usuario.Email)
      };

      SecurityTokenDescriptor tokenDescriptor = new()
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(1),
        Issuer = _configuration["Jwt:Issuer"],
        Audience = _configuration["Jwt:Audience"],
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
      };

      JwtSecurityTokenHandler tokenHandler = new();
      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}
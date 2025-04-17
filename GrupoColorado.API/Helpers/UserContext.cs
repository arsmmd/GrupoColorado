using GrupoColorado.API.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace GrupoColorado.API.Helpers
{
  public class UserContext : IUserContext
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserContext> _logger;

    public UserContext(IHttpContextAccessor httpContextAccessor, ILogger<UserContext> logger)
    {
      _httpContextAccessor = httpContextAccessor;
      _logger = logger;
    }

    public string GetUserClaim(string typeName)
    {
      try
      {
        ClaimsPrincipal user = _httpContextAccessor.HttpContext?.User;
        string claim = user?.FindFirst(typeName)?.Value;
        return claim;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);
      }

      return null;
    }
  }
}
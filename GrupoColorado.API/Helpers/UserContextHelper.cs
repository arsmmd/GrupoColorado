using GrupoColorado.API.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace GrupoColorado.API.Helpers
{
  public class UserContextHelper : IUserContextHelper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserContextHelper> _logger;

    public UserContextHelper(IHttpContextAccessor httpContextAccessor, ILogger<UserContextHelper> logger)
    {
      _httpContextAccessor = httpContextAccessor;
      _logger = logger;
    }

    public string GetUserClaim(string typeName)
    {
      try
      {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(typeName)?.Value;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);
      }

      return null;
    }
  }
}
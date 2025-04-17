using GrupoColorado.API.Helpers.Interfaces;
using System.Security.Claims;

namespace GrupoColorado.API.Extensions
{
  public static class UserContextExtensions
  {
    public static int GetNameIdentifierAsInt(this IUserContext userContext)
    {
      return int.TryParse(userContext.GetUserClaim(ClaimTypes.NameIdentifier), out int codigoUsuario) ? codigoUsuario : 0;
    }
  }
}
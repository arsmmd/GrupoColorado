namespace GrupoColorado.API.Helpers.Interfaces
{
  public interface IUserContext
  {
    string GetUserClaim(string typeName);
  }
}
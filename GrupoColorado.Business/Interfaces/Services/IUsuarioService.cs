using GrupoColorado.Business.Entities;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Interfaces.Services
{
  public interface IUsuarioService : IBaseService<Usuario>
  {
    Task DeleteAsync(Usuario entity, int codigoUsuario);
    Task<Usuario> AuthenticateAsync(string email, string senha);
  }
}
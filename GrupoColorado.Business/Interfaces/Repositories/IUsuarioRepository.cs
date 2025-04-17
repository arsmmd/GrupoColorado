using GrupoColorado.Business.Entities;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Interfaces.Repositories
{
  public interface IUsuarioRepository : IBaseRepository<Usuario>
  {
    Task<Usuario> GetByEmailAsync(string email);
  }
}
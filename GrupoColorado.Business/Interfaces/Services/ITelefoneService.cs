using GrupoColorado.Business.Entities;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Interfaces.Services
{
  public interface ITelefoneService : IBaseService<Telefone>
  {
    Task<GrupoColorado.Business.Shared.PagedResults<Telefone>> GetPagedAsync(int codigoCliente, GrupoColorado.Business.Shared.QueryParameters queryParameters);
  }
}
using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Business.Interfaces.Services;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Services
{
  public class TipoTelefoneService : BaseService<TipoTelefone>, ITipoTelefoneService
  {
    private readonly ITipoTelefoneRepository _tipoTelefoneRepository;

    public TipoTelefoneService(ITipoTelefoneRepository tipoTelefoneRepository) : base(tipoTelefoneRepository)
    {
      _tipoTelefoneRepository = tipoTelefoneRepository;
    }

    public override async Task<GrupoColorado.Business.Shared.PagedResults<TipoTelefone>> GetPagedAsync(GrupoColorado.Business.Shared.QueryParameters queryParameters)
    {
      return await _tipoTelefoneRepository.GetPagedAsync(queryParameters, c => c.Usuario);
    }
  }
}
using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Business.Interfaces.Services;

namespace GrupoColorado.Business.Services
{
  public class TipoTelefoneService : BaseService<TipoTelefone>, ITipoTelefoneService
  {
    public TipoTelefoneService(ITipoTelefoneRepository tipoTelefoneRepository) : base(tipoTelefoneRepository)
    {
    }
  }
}
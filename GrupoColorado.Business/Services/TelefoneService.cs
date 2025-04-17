using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Business.Interfaces.Services;

namespace GrupoColorado.Business.Services
{
  public class TelefoneService : BaseService<Telefone>, ITelefoneService
  {
    public TelefoneService(ITelefoneRepository telefoneRepository) : base(telefoneRepository)
    {
    }
  }
}
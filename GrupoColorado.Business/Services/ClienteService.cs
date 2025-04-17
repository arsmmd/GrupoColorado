using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Business.Interfaces.Services;

namespace GrupoColorado.Business.Services
{
  public class ClienteService : BaseService<Cliente>, IClienteService
  {
    public ClienteService(IClienteRepository clienteRepository) : base(clienteRepository)
    {
    }
  }
}
using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Business.Interfaces.Services;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Services
{
  public class ClienteService : BaseService<Cliente>, IClienteService
  {
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository) : base(clienteRepository)
    {
      _clienteRepository = clienteRepository;
    }

    public override async Task<GrupoColorado.Business.Shared.PagedResults<Cliente>> GetPagedAsync(GrupoColorado.Business.Shared.QueryParameters queryParameters)
    {
      return await _clienteRepository.GetPagedAsync(queryParameters, c => c.Usuario);
    }
  }
}
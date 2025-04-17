using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Business.Interfaces.Services;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Services
{
  public class TelefoneService : BaseService<Telefone>, ITelefoneService
  {
    private readonly ITelefoneRepository _telefoneRepository;

    public TelefoneService(ITelefoneRepository telefoneRepository) : base(telefoneRepository)
    {
      _telefoneRepository = telefoneRepository;
    }

    public async Task<GrupoColorado.Business.Shared.PagedResults<Telefone>> GetPagedAsync(int codigoCliente, GrupoColorado.Business.Shared.QueryParameters queryParameters)
    {
      if (codigoCliente > 0)
      {
        if (queryParameters.Filters.ContainsKey("codigoCliente"))
          queryParameters.Filters.Remove("codigoCliente");

        queryParameters.Filters.Add("codigoCliente", codigoCliente.ToString());
      }

      return await _telefoneRepository.GetPagedAsync(queryParameters, t => t.Cliente, t => t.TipoTelefone, t => t.Usuario);
    }
  }
}
using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace GrupoColorado.Infrastructure.Repositories
{
  public class TipoTelefoneRepository : BaseRepository<TipoTelefone>, ITipoTelefoneRepository
  {
    public TipoTelefoneRepository(AppDbContext context, ILogger<TipoTelefoneRepository> logger) : base(context, logger)
    {
    }
  }
}
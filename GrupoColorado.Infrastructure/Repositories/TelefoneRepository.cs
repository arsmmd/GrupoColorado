using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace GrupoColorado.Infrastructure.Repositories
{
  public class TelefoneRepository : BaseRepository<Telefone>, ITelefoneRepository
  {
    public TelefoneRepository(AppDbContext context, ILogger<TelefoneRepository> logger) : base(context, logger)
    {
    }
  }
}
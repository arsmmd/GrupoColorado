using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace GrupoColorado.Infrastructure.Repositories
{
  public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
  {
    public ClienteRepository(AppDbContext context, ILogger<ClienteRepository> logger) : base(context, logger)
    {
    }
  }
}
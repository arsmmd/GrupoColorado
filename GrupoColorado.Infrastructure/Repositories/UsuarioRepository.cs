using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GrupoColorado.Infrastructure.Repositories
{
  public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
  {
    public UsuarioRepository(AppDbContext context, ILogger<UsuarioRepository> logger) : base(context, logger)
    {
    }

    public override async Task<GrupoColorado.Business.Shared.PagedResults<Usuario>> GetPagedAsync(GrupoColorado.Business.Shared.QueryParameters queryParameters, params Expression<Func<Usuario, object>>[] includes)
    {
      GrupoColorado.Business.Shared.PagedResults<Usuario> pagedResults = await base.GetPagedAsync(queryParameters, includes);
      foreach (var item in pagedResults.Items)
        item.Senha = null;

      return pagedResults;
    }

    public override Usuario GetByPk(params object[] key)
    {
      Usuario result = base.GetByPk(key);
      result.Senha = null;
      return result;
    }

    public override async Task<Usuario> GetByPkAsync(params object[] key)
    {
      Usuario result = await base.GetByPkAsync(key);
      result.Senha = null;
      return result;
    }

    public override void Update(Usuario entity)
    {
      Usuario usuario = base.GetByPk(entity.CodigoUsuario);
      entity.Senha = usuario.Senha; // Para garantir que a senha não será atualizada através de um UPDATE.
      base.Update(entity);
    }

    public async Task<Usuario> GetByEmailAsync(string email)
    {
      return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
  }
}
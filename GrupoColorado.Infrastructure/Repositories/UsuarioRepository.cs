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
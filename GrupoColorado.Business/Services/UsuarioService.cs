using GrupoColorado.Application.Helpers;
using GrupoColorado.Business.Entities;
using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Business.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Services
{
  public class UsuarioService : BaseService<Usuario>, IUsuarioService
  {
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository) : base(usuarioRepository)
    {
      _usuarioRepository = usuarioRepository;
    }

    public override async Task AddAsync(Usuario entity)
    {
      entity.Senha = SecurePasswordHelper.HashPassword(entity.Senha);
      await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(Usuario entity)
    {
      // OBS: Futuramente implementar a troca de senha em outro método e através de mecanismos corretos, como um envio de e-mail com link, etc...

      Usuario usuario = await base.GetByPkAsync(entity.CodigoUsuario);
      entity.Senha = usuario.Senha; // Para garantir que a senha não será atualizada através de um UPDATE.
      await base.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Usuario entity, int codigoUsuario)
    {
      if (entity.CodigoUsuario == codigoUsuario)
        throw new InvalidOperationException("Você não pode excluir seu próprio usuário!");

      await base.DeleteAsync(entity);
    }

    public async Task<Usuario> AuthenticateAsync(string email, string senha)
    {
      Usuario usuario = await _usuarioRepository.GetByEmailAsync(email);
      if ((usuario == null) || (!(usuario.Ativo)) || (!SecurePasswordHelper.VerifyPassword(senha, usuario.Senha)))
        return null;

      return usuario;
    }
  }
}
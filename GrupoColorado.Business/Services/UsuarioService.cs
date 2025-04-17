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

    public async Task DeleteAsync(Usuario entity, int codigoUsuario)
    {
      if (entity.CodigoUsuario == codigoUsuario)
        throw new InvalidOperationException("Você não pode excluir seu próprio usuário!");

      await base.DeleteAsync(entity);
    }

    public async Task<Usuario> AuthenticateAsync(string email, string senha)
    {
      Usuario usuario = await _usuarioRepository.GetByEmailAsync(email);
      if ((usuario == null) || (!(usuario.Ativo)) || (usuario.Senha != senha))
        return null;

      return usuario;
    }
  }
}
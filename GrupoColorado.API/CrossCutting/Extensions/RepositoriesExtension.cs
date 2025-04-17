using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GrupoColorado.API.CrossCutting.Extensions
{
  public static class RepositoriesExtension
  {
    public static void RegisterRepositories(this IServiceCollection services)
    {
      services.AddScoped<IClienteRepository, ClienteRepository>();
      services.AddScoped<ITelefoneRepository, TelefoneRepository>();
      services.AddScoped<ITipoTelefoneRepository, TipoTelefoneRepository>();
      services.AddScoped<IUsuarioRepository, UsuarioRepository>();
    }
  }
}
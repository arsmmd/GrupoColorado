using GrupoColorado.Business.Interfaces.Services;
using GrupoColorado.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GrupoColorado.API.CrossCutting.Extensions
{
  public static class ServicesExtension
  {
    public static void RegisterServices(this IServiceCollection services)
    {
      services.AddScoped<IClienteService, ClienteService>();
      services.AddScoped<ITelefoneService, TelefoneService>();
      services.AddScoped<ITipoTelefoneService, TipoTelefoneService>();
      services.AddScoped<IUsuarioService, UsuarioService>();
    }
  }
}
using GrupoColorado.API.CrossCutting.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GrupoColorado.API.CrossCutting
{
  public static class IoC
  {
    public static void RegisterValidators(IServiceCollection services) => services.RegisterValidators();
    public static void RegisterServices(IServiceCollection services) => services.RegisterServices();
    public static void RegisterRepositories(IServiceCollection services) => services.RegisterRepositories();
  }
}
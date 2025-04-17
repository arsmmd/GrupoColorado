using FluentValidation;
using GrupoColorado.API.DTOs.Validators;
using GrupoColorado.API.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace GrupoColorado.API.CrossCutting.Extensions
{
  public static class ValidatorsExtension
  {
    public static void RegisterValidators(this IServiceCollection services)
    {
      services.AddSingleton<ValidationFilter>();

      services.AddValidatorsFromAssemblyContaining<ClienteDtoValidator>();
      services.AddValidatorsFromAssemblyContaining<TelefoneDtoValidator>();
      services.AddValidatorsFromAssemblyContaining<TipoTelefoneDtoValidator>();
      services.AddValidatorsFromAssemblyContaining<UsuarioDtoValidator>();
    }
  }
}
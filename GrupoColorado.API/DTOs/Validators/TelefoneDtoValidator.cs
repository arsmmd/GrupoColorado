using FluentValidation;

namespace GrupoColorado.API.DTOs.Validators
{
  public class TelefoneDtoValidator : AbstractValidator<TelefoneDto>
  {
    public TelefoneDtoValidator()
    {
      RuleFor(x => x.CodigoCliente)
          .NotEmpty().WithMessage("O código do cliente é obrigatório.");

      RuleFor(x => x.NumeroTelefone)
          .NotEmpty().WithMessage("O número do telefone é obrigatório.")
          .MaximumLength(11).WithMessage("O tamanho máximo é 11 caracteres.");

      RuleFor(x => x.CodigoTipoTelefone)
          .NotEmpty().WithMessage("O código do tipo do telefone é obrigatório.");

      RuleFor(x => x.Operadora)
          .NotEmpty().WithMessage("A operadora é obrigatória.")
          .MaximumLength(100).WithMessage("O tamanho máximo é 100 caracteres.");
    }
  }
}
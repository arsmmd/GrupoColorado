using FluentValidation;

namespace GrupoColorado.API.DTOs.Validators
{
  public class TelefoneDtoValidator : AbstractValidator<TelefoneDto>
  {
    public TelefoneDtoValidator()
    {
      RuleFor(x => x.CodigoCliente)
          .NotEmpty().WithMessage("O c�digo do cliente � obrigat�rio.");

      RuleFor(x => x.NumeroTelefone)
          .NotEmpty().WithMessage("O n�mero do telefone � obrigat�rio.")
          .MaximumLength(11).WithMessage("O tamanho m�ximo � 11 caracteres.");

      RuleFor(x => x.CodigoTipoTelefone)
          .NotEmpty().WithMessage("O c�digo do tipo do telefone � obrigat�rio.");

      RuleFor(x => x.Operadora)
          .NotEmpty().WithMessage("A operadora � obrigat�ria.")
          .MaximumLength(100).WithMessage("O tamanho m�ximo � 100 caracteres.");
    }
  }
}
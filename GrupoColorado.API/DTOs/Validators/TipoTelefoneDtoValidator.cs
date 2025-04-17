using FluentValidation;

namespace GrupoColorado.API.DTOs.Validators
{
  public class TipoTelefoneDtoValidator : AbstractValidator<TipoTelefoneDto>
  {
    public TipoTelefoneDtoValidator()
    {
      RuleFor(x => x.DescricaoTipoTelefone)
          .NotEmpty().WithMessage("A descrição é obrigatória.")
          .MaximumLength(80).WithMessage("O tamanho máximo é 80 caracteres.");
    }
  }
}
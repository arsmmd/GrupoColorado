using FluentValidation;

namespace GrupoColorado.API.DTOs.Validators
{
  public class TipoTelefoneDtoValidator : AbstractValidator<TipoTelefoneDto>
  {
    public TipoTelefoneDtoValidator()
    {
      RuleFor(x => x.DescricaoTipoTelefone)
          .NotEmpty().WithMessage("A descri��o � obrigat�ria.")
          .MaximumLength(80).WithMessage("O tamanho m�ximo � 80 caracteres.");
    }
  }
}
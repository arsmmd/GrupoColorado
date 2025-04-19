using FluentValidation;

namespace GrupoColorado.API.DTOs.Validators
{
  public class UsuarioDtoValidator : AbstractValidator<UsuarioDto>
  {
    public UsuarioDtoValidator()
    {
      RuleFor(x => x.Nome)
          .NotEmpty().WithMessage("O nome � obrigat�rio.")
          .MaximumLength(50).WithMessage("O tamanho m�ximo � 50 caracteres.");

      RuleFor(x => x.Email)
          .NotEmpty().WithMessage("O e-mail � obrigat�rio.")
          .EmailAddress().WithMessage("E-mail inv�lido.")
          .MaximumLength(250).WithMessage("O tamanho m�ximo � 250 caracteres.");

      RuleFor(x => x.Senha)
          .NotEmpty().WithMessage("A senha � obrigat�ria.")
          .MaximumLength(64).WithMessage("O tamanho m�ximo � 64 caracteres.");
    }
  }
}
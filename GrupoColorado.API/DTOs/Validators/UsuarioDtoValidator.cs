using FluentValidation;

namespace GrupoColorado.API.DTOs.Validators
{
  public class UsuarioDtoValidator : AbstractValidator<UsuarioDto>
  {
    public UsuarioDtoValidator()
    {
      RuleFor(x => x.Nome)
          .NotEmpty().WithMessage("O nome é obrigatório.")
          .MaximumLength(50).WithMessage("O tamanho máximo é 50 caracteres.");

      RuleFor(x => x.Email)
          .NotEmpty().WithMessage("O e-mail é obrigatório.")
          .EmailAddress().WithMessage("E-mail inválido.")
          .MaximumLength(250).WithMessage("O tamanho máximo é 250 caracteres.");

      RuleFor(x => x.Senha)
          .NotEmpty().WithMessage("A senha é obrigatória.")
          .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
          .MaximumLength(50).WithMessage("O tamanho máximo é 50 caracteres.");
    }
  }
}
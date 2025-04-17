using FluentValidation;

namespace GrupoColorado.API.DTOs.Validators
{
  public class ClienteDtoValidator : AbstractValidator<ClienteDto>
  {
    public ClienteDtoValidator()
    {
      RuleFor(x => x.RazaoSocial)
          .NotEmpty().WithMessage("A razão social é obrigatória.")
          .MaximumLength(100).WithMessage("O tamanho máximo é 100 caracteres.");

      RuleFor(x => x.NomeFantasia)
          .NotEmpty().WithMessage("O nome fantasia é obrigatório.")
          .MaximumLength(100).WithMessage("O tamanho máximo é 100 caracteres.");

      RuleFor(x => x.TipoPessoa)
          .NotEmpty().WithMessage("O tipo da pessoa é obrigatório.")
          .MinimumLength(1).WithMessage("O tamanho mínimo é 1 caractere.")
          .MaximumLength(1).WithMessage("O tamanho máximo é 1 caractere.");

      RuleFor(x => x.Documento)
          .NotEmpty().WithMessage("O documento é obrigatório.")
          .MaximumLength(14).WithMessage("O tamanho máximo é 14 caracteres.");

      RuleFor(x => x.Endereco)
          .NotEmpty().WithMessage("O endereço é obrigatório.")
          .MaximumLength(100).WithMessage("O tamanho máximo é 100 caracteres.");

      RuleFor(x => x.Complemento)
          .MaximumLength(100).WithMessage("O tamanho máximo é 100 caracteres.");

      RuleFor(x => x.Bairro)
          .NotEmpty().WithMessage("O bairro é obrigatório.")
          .MaximumLength(100).WithMessage("O tamanho máximo é 100 caracteres.");

      RuleFor(x => x.Cidade)
          .NotEmpty().WithMessage("A cidade é obrigatória.")
          .MaximumLength(100).WithMessage("O tamanho máximo é 100 caracteres.");

      RuleFor(x => x.CEP)
          .NotEmpty().WithMessage("O CEP é obrigatório.")
          .MinimumLength(8).WithMessage("O tamanho mínimo é 8 caracteres.")
          .MaximumLength(8).WithMessage("O tamanho máximo é 8 caracteres.");

      RuleFor(x => x.UF)
          .NotEmpty().WithMessage("A UF é obrigatória.")
          .MinimumLength(2).WithMessage("O tamanho mínimo é 2 caracteres.")
          .MaximumLength(2).WithMessage("O tamanho máximo é 2 caracteres.");
    }
  }
}
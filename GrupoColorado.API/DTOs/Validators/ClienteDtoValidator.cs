using FluentValidation;

namespace GrupoColorado.API.DTOs.Validators
{
  public class ClienteDtoValidator : AbstractValidator<ClienteDto>
  {
    public ClienteDtoValidator()
    {
      RuleFor(x => x.RazaoSocial)
          .NotEmpty().WithMessage("A raz�o social � obrigat�ria.")
          .MaximumLength(100).WithMessage("O tamanho m�ximo � 100 caracteres.");

      RuleFor(x => x.NomeFantasia)
          .NotEmpty().WithMessage("O nome fantasia � obrigat�rio.")
          .MaximumLength(100).WithMessage("O tamanho m�ximo � 100 caracteres.");

      RuleFor(x => x.TipoPessoa)
          .NotEmpty().WithMessage("O tipo da pessoa � obrigat�rio.")
          .MinimumLength(1).WithMessage("O tamanho m�nimo � 1 caractere.")
          .MaximumLength(1).WithMessage("O tamanho m�ximo � 1 caractere.");

      RuleFor(x => x.Documento)
          .NotEmpty().WithMessage("O documento � obrigat�rio.")
          .MaximumLength(14).WithMessage("O tamanho m�ximo � 14 caracteres.");

      RuleFor(x => x.Endereco)
          .NotEmpty().WithMessage("O endere�o � obrigat�rio.")
          .MaximumLength(100).WithMessage("O tamanho m�ximo � 100 caracteres.");

      RuleFor(x => x.Complemento)
          .MaximumLength(100).WithMessage("O tamanho m�ximo � 100 caracteres.");

      RuleFor(x => x.Bairro)
          .NotEmpty().WithMessage("O bairro � obrigat�rio.")
          .MaximumLength(100).WithMessage("O tamanho m�ximo � 100 caracteres.");

      RuleFor(x => x.Cidade)
          .NotEmpty().WithMessage("A cidade � obrigat�ria.")
          .MaximumLength(100).WithMessage("O tamanho m�ximo � 100 caracteres.");

      RuleFor(x => x.CEP)
          .NotEmpty().WithMessage("O CEP � obrigat�rio.")
          .MinimumLength(8).WithMessage("O tamanho m�nimo � 8 caracteres.")
          .MaximumLength(8).WithMessage("O tamanho m�ximo � 8 caracteres.");

      RuleFor(x => x.UF)
          .NotEmpty().WithMessage("A UF � obrigat�ria.")
          .MinimumLength(2).WithMessage("O tamanho m�nimo � 2 caracteres.")
          .MaximumLength(2).WithMessage("O tamanho m�ximo � 2 caracteres.");
    }
  }
}
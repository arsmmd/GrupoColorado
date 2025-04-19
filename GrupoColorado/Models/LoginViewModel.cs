using System.ComponentModel.DataAnnotations;

namespace GrupoColorado.Models
{
  public class LoginViewModel
  {
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
  }
}
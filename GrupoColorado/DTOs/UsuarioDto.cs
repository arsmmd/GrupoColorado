using System;

namespace GrupoColorado.DTOs
{
  public class UsuarioDto
  {
    public int CodigoUsuario { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataInsercao { get; set; }
  }
}
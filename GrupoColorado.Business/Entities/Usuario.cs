using System;

namespace GrupoColorado.Business.Entities
{
  public class Usuario
  {
    public int CodigoUsuario { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataInsercao { get; set; }
  }
}
using System;
using System.Collections.Generic;

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

    public ICollection<Cliente> Clientes { get; set; }
    public ICollection<Telefone> Telefones { get; set; }
    public ICollection<TipoTelefone> TiposTelefone { get; set; }
  }
}
using System;
using System.Collections.Generic;

namespace GrupoColorado.Business.Entities
{
  public class TipoTelefone
  {
    public int CodigoTipoTelefone { get; set; }
    public string DescricaoTipoTelefone { get; set; }
    public DateTime DataInsercao { get; set; }
    public int UsuarioInsercao { get; set; }

    public ICollection<Telefone> Telefones { get; set; }
    public Usuario Usuario { get; set; }
  }
}
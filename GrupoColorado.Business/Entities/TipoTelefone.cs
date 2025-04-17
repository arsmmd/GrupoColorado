using System;

namespace GrupoColorado.Business.Entities
{
  public class TipoTelefone
  {
    public int CodigoTipoTelefone { get; set; }
    public string DescricaoTipoTelefone { get; set; }
    public DateTime DataInsercao { get; set; }
    public int UsuarioInsercao { get; set; }
  }
}
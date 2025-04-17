using System;

namespace GrupoColorado.DTOs
{
  public class TipoTelefoneDto
  {
    public int CodigoTipoTelefone { get; set; }
    public string DescricaoTipoTelefone { get; set; }
    public DateTime DataInsercao { get; set; }
    public int UsuarioInsercao { get; set; }

    public UsuarioDto Usuario { get; set; }
  }
}
using System;

namespace GrupoColorado.API.DTOs
{
  public class TipoTelefoneDto
  {
    public string DescricaoTipoTelefone { get; set; }

    public int CodigoTipoTelefone { get; internal set; }
    public DateTime DataInsercao { get; internal set; }
    public int UsuarioInsercao { get; internal set; }

    public UsuarioDto Usuario { get; internal set; }
  }
}
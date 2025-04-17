using System;

namespace GrupoColorado.DTOs
{
  public class TelefoneDto
  {
    public int CodigoCliente { get; set; }
    public string NumeroTelefone { get; set; }
    public int CodigoTipoTelefone { get; set; }
    public string Operadora { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataInsercao { get; set; }
    public int UsuarioInsercao { get; set; }

    public TipoTelefoneDto TipoTelefone { get; set; }
  }
}
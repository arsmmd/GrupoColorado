using System;

namespace GrupoColorado.API.DTOs
{
  public class TelefoneDto
  {
    public int CodigoCliente { get; set; }
    public string NumeroTelefone { get; set; }
    public int CodigoTipoTelefone { get; set; }
    public string Operadora { get; set; }
    public bool Ativo { get; set; }

    public DateTime DataInsercao { get; internal set; }
    public int UsuarioInsercao { get; internal set; }
  }
}
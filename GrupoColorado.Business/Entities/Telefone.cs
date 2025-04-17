using System;

namespace GrupoColorado.Business.Entities
{
  public class Telefone
  {
    public int CodigoCliente { get; set; }
    public string NumeroTelefone { get; set; }
    public int CodigoTipoTelefone { get; set; }
    public string Operadora { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataInsercao { get; set; }
    public int UsuarioInsercao { get; set; }
  }
}
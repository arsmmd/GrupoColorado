using GrupoColorado.Application.Helpers;
using System;
using System.Text.Json.Serialization;

namespace GrupoColorado.API.DTOs
{
  public class UsuarioDto
  {
    public string Nome { get; set; }
    public string Email { get; set; }
    
    [JsonConverter(typeof(ReadOnlyJsonConverter<string>))]
    public string Senha { get; set; }

    public bool Ativo { get; set; }
    public int CodigoUsuario { get; internal set; }
    public DateTime DataInsercao { get; internal set; }
  }
}
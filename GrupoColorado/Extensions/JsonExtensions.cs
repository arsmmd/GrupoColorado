using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GrupoColorado.Extensions
{
  public static class JsonExtensions
  {
    public static string Serialize<T>(this T model)
    {
      return JsonSerializer.Serialize(model, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles });
    }

    public static T Deserialize<T>(this string json)
    {
      return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles });
    }
  }
}
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GrupoColorado.Application.Helpers
{
  public class ReadOnlyJsonConverter<T> : JsonConverter<T>
  {
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => JsonSerializer.Deserialize<T>(ref reader, options);
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, default, typeof(T), options);
  }
}
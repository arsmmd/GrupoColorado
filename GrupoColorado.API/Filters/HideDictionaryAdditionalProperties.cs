using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace GrupoColorado.API.Filters
{
    public class HideDictionaryAdditionalProperties : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(Dictionary<string, string>))
            {
                schema.AdditionalPropertiesAllowed = false;
                schema.Properties.Clear();
                schema.Description = "Exemplo para filtro: { \"campo1\": \"valor1\", \"campo2\": \"valor2\" }";
            }
        }
    }
}
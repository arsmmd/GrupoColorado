using System;
using System.Collections.Generic;
using System.Reflection;

namespace GrupoColorado.Extensions
{
  public static class QueryStringExtensions
  {
    public static string ToQueryString(this object obj)
    {
      if (obj == null)
        return string.Empty;

      List<string> queryParams = new();

      PropertyInfo[] properties = obj.GetType().GetProperties();
      foreach (PropertyInfo property in properties)
      {
        object value = property.GetValue(obj);
        if (value == null)
          continue;

        string name = property.Name;
        if (value is not IDictionary<string, string> dictionary)
          queryParams.Add($"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(value.ToString())}");
        else
          foreach (KeyValuePair<string, string> kvp in dictionary)
          {
            string key = Uri.EscapeDataString(kvp.Key);
            string val = Uri.EscapeDataString(kvp.Value ?? string.Empty);
            queryParams.Add($"{name}[{key}]={val}");
          }
      }

      return string.Join("&", queryParams);
    }
  }
}
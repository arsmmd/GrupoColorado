using System.Collections.Generic;
using System.Linq;

namespace GrupoColorado.DTOs.Core
{
  public class ValidationError
  {
    public string Property { get; set; }
    public List<string> Messages { get; set; }

    public static string FormatOutput(List<ValidationError> errors)
    {
      if (errors == null || !errors.Any())
        return string.Empty;

      List<string> outputMessages = new();
      foreach (ValidationError error in errors)
        foreach (string msg in error.Messages)
          outputMessages.Add($"{error.Property}: {msg}");

      return string.Join(" <br> ", outputMessages);
    }
  }
}
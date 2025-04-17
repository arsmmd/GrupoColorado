using System.Collections.Generic;

namespace GrupoColorado.Business.Shared
{
  public class QueryParameters
  {
    public QueryParameters()
    {
      this.Filters = new();
      this.OrderDescending = false;
      this.Page = 1;
      this.PageSize = 10;
    }

    public Dictionary<string, string> Filters { get; set; }

    public string OrderBy { get; set; }
    public bool OrderDescending { get; set; } = false;

    public int Page { get; set; }
    public int PageSize { get; set; }
  }
}
using System.Collections.Generic;

namespace GrupoColorado.Business.Shared
{
  public class PagedResults<T>
  {
    public IEnumerable<T> Items { get; set; }
    public int Count { get; set; }
  }
}
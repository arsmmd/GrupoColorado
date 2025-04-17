using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Interfaces.Services
{
  public interface IBaseService<T>
  {
    Task<GrupoColorado.Business.Shared.PagedResults<T>> GetPagedAsync(GrupoColorado.Business.Shared.QueryParameters queryParameters, params Expression<Func<T, object>>[] includes);
    Task<T> GetByPkAsync(params object[] key);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
  }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Interfaces.Repositories
{
  public interface IBaseRepository<T>
  {
    Task<GrupoColorado.Business.Shared.PagedResults<T>> GetPagedAsync(GrupoColorado.Business.Shared.QueryParameters queryParameters);
    T GetByPk(params object[] key);
    Task<T> GetByPkAsync(params object[] key);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
  }
}
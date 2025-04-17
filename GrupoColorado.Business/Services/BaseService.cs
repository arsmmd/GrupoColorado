using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Business.Interfaces.Services;
using System.Threading.Tasks;

namespace GrupoColorado.Business.Services
{
  public abstract class BaseService<T> : IBaseService<T> where T : class
  {
    private readonly IBaseRepository<T> _repository;

    protected BaseService(IBaseRepository<T> repository)
    {
      _repository = repository;
    }

    public virtual async Task<GrupoColorado.Business.Shared.PagedResults<T>> GetPagedAsync(GrupoColorado.Business.Shared.QueryParameters queryParameters)
    {
      return await _repository.GetPagedAsync(queryParameters);
    }

    public virtual async Task<T> GetByPkAsync(params object[] key)
    {
      return await _repository.GetByPkAsync(key);
    }

    public virtual async Task AddAsync(T entity)
    {
      await _repository.AddAsync(entity);
      await _repository.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
      _repository.Update(entity);
      await _repository.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
      _repository.Delete(entity);
      await _repository.SaveChangesAsync();
    }
  }
}
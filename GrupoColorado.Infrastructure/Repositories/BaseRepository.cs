using GrupoColorado.Business.Interfaces.Repositories;
using GrupoColorado.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace GrupoColorado.Infrastructure.Repositories
{
  public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
  {
    protected readonly AppDbContext _context;
    protected readonly ILogger<BaseRepository<T>> _logger;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(AppDbContext context, ILogger<BaseRepository<T>> logger)
    {
      _context = context;
      _logger = logger;
      _dbSet = _context.Set<T>();
    }

    public virtual async Task<GrupoColorado.Business.Shared.PagedResults<T>> GetPagedAsync(GrupoColorado.Business.Shared.QueryParameters queryParameters)
    {
      IQueryable<T> query = _dbSet.AsQueryable();

      foreach (KeyValuePair<string, string> filter in queryParameters.Filters)
      {
        string propertyName = filter.Key;
        string value = filter.Value;

        System.Reflection.PropertyInfo property = typeof(T).GetProperty(propertyName, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        if (property != null && !string.IsNullOrWhiteSpace(value))
        {
          try
          {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression member = Expression.Property(parameter, property.Name);

            if (property.PropertyType == typeof(string))
            {
              MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
              MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

              MethodCallExpression toLowerMember = Expression.Call(member, toLowerMethod);
              ConstantExpression constant = Expression.Constant(value.ToLower());
              MethodCallExpression containsExpression = Expression.Call(toLowerMember, containsMethod, constant);

              Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
              query = query.Where(lambda);
            }
            else
            {
              Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
              object typedValue = Convert.ChangeType(value, propertyType);
              ConstantExpression constant = Expression.Constant(typedValue);
              BinaryExpression body = Expression.Equal(member, constant);
              Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
              query = query.Where(lambda);
            }
          }
          catch
          {
            // Ignora erros de conversão ou construção de expressão
          }
        }
      }

      // Ordenação
      if (!string.IsNullOrWhiteSpace(queryParameters.OrderBy))
        query = query.OrderBy($"{queryParameters.OrderBy} {(queryParameters.OrderDescending ? "descending" : "ascending")}");

      int totalCount = await query.CountAsync();

      if (totalCount == 0)
        return new GrupoColorado.Business.Shared.PagedResults<T>() { Count = 0 };

      // Paginação
      List<T> items = await query
          .AsNoTracking()
          .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
          .Take(queryParameters.PageSize)
          .ToListAsync();

      return new GrupoColorado.Business.Shared.PagedResults<T>
      {
        Items = items,
        Count = totalCount
      };
    }

    public virtual T GetByPk(params object[] key)
    {
      IKey pk = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey();
      IQueryable<T> query = _dbSet.AsQueryable();

      for (int i = 0; i < pk.Properties.Count; i++)
      {
        IProperty property = pk.Properties[i];
        object value = key[i];

        query = query.Where(e => EF.Property<object>(e, property.Name).Equals(value));
      }

      return query.AsNoTracking().FirstOrDefault();
    }

    public virtual async Task<T> GetByPkAsync(params object[] key)
    {
      IKey pk = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey();
      IQueryable<T> query = _dbSet.AsQueryable();

      for (int i = 0; i < pk.Properties.Count; i++)
      {
        IProperty property = pk.Properties[i];
        object value = key[i];

        query = query.Where(e => EF.Property<object>(e, property.Name).Equals(value));
      }

      return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
      await _dbSet.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
      _context.Entry(entity).State = EntityState.Modified;
      PropertyEntry dataInsercaoProperty = _context.Entry(entity).Properties.FirstOrDefault(p => p.Metadata.Name == "DataInsercao");
      if (dataInsercaoProperty != null)
        dataInsercaoProperty.IsModified = false;

      PropertyEntry usuarioInsercaoProperty = _context.Entry(entity).Properties.FirstOrDefault(p => p.Metadata.Name == "UsuarioInsercao");
      if (usuarioInsercaoProperty != null)
        usuarioInsercaoProperty.IsModified = false;
    }

    public virtual void Delete(T entity)
    {
      _dbSet.Remove(entity);
    }

    public virtual async Task SaveChangesAsync()
    {
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException ex) when (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE"))
      {
        _logger.LogError(ex, ex.Message);
        throw new InvalidOperationException("Operação inválida: a entidade está relacionada a outra e não pode ser removida.");
      }
      catch (Exception ex)
      {
        _logger.LogCritical(ex, ex.Message);
        throw new Exception("Erro ao salvar as alterações no banco de dados. Consulte os logs para mais detalhes.", ex);
      }
    }
  }
}
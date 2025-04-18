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

    public virtual async Task<GrupoColorado.Business.Shared.PagedResults<T>> GetPagedAsync(GrupoColorado.Business.Shared.QueryParameters queryParameters, params Expression<Func<T, object>>[] includes)
    {
      IQueryable<T> query = _dbSet.AsQueryable();

      // Filtragem din�mica dos resultados
      foreach (KeyValuePair<string, string> filter in queryParameters.Filters)
      {
        string propertyName = filter.Key;
        string value = filter.Value;

        if (string.IsNullOrWhiteSpace(value))
          continue;

        try
        {
          ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
          Expression propertyAccess = parameter;

          Type type = typeof(T);
          foreach (var part in propertyName.Split('.'))
          {
            var propertyInfo = type.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
              propertyAccess = null;
              break;
            }

            propertyAccess = Expression.Property(propertyAccess, propertyInfo);
            type = propertyInfo.PropertyType;
          }

          if (propertyAccess == null)
            continue;

          if (type == typeof(string))
          {
            MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            var toLower = Expression.Call(propertyAccess, toLowerMethod);
            var constant = Expression.Constant(value.ToLower());
            var contains = Expression.Call(toLower, containsMethod, constant);

            var lambda = Expression.Lambda<Func<T, bool>>(contains, parameter);
            query = query.Where(lambda);
          }
          else
          {
            Type propertyType = Nullable.GetUnderlyingType(type) ?? type;
            object typedValue = Convert.ChangeType(value, propertyType);
            ConstantExpression constant = Expression.Constant(typedValue);
            BinaryExpression equal = Expression.Equal(propertyAccess, constant);

            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);
            query = query.Where(lambda);
          }
        }
        catch
        {
          // Ignora erros de convers�o ou constru��o de express�o
        }
      }

      // Ordena��o
      if (!string.IsNullOrWhiteSpace(queryParameters.OrderBy))
        query = query.OrderBy($"{queryParameters.OrderBy} {(queryParameters.OrderDescending ? "descending" : "ascending")}");


      // Inclus�o das propriedades aninhadas (Navigations)
      if (includes != null)
        foreach (Expression<Func<T, object>> include in includes)
          query = query.Include(include);


      int totalCount = await query.CountAsync();
      if (totalCount == 0)
        return new GrupoColorado.Business.Shared.PagedResults<T>() { Count = 0 };

      // Pagina��o
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
        throw new InvalidOperationException("Opera��o inv�lida: a entidade est� relacionada a outra e n�o pode ser removida.");
      }
      catch (Exception ex)
      {
        _logger.LogCritical(ex, ex.Message);
        throw new Exception("Erro ao salvar as altera��es no banco de dados. Consulte os logs para mais detalhes.", ex);
      }
    }
  }
}
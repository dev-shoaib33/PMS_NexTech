using Microsoft.EntityFrameworkCore;
using PMS.DB.Model.Data;
using PMS.Repositories.Interfaces;
using System.Linq.Expressions;

namespace PMS.Repositories.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private DbSet<T> _dbSet;
    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        if (includeProperties != null)
        {
            foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }

        //var entities = query.ToList();

        var filteredEntities = query.AsEnumerable().Where(entity => GetActiveFlagValue(entity));

        if (predicate != null)
        {
            filteredEntities = filteredEntities.Where(predicate.Compile());
        }

        return filteredEntities;
    }

    public T GetT(Expression<Func<T, bool>> predicate, string includeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }

        var filteredEntities = query.AsEnumerable().Where(entity => GetActiveFlagValue(entity));

        return filteredEntities.AsQueryable().FirstOrDefault(predicate);
    }

    private bool GetActiveFlagValue(T entity)
    {
        var activeFlagProperty = entity.GetType().GetProperty("ActiveFlag");
        if (activeFlagProperty != null && activeFlagProperty.PropertyType == typeof(bool))
        {
            return (bool)activeFlagProperty.GetValue(entity);
        }
        return false; 
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
    public void DeleteRange(IEnumerable<T> entity)
    {
        _dbSet.RemoveRange(entity);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}

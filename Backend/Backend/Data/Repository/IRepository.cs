using System;
namespace Data.Repository;

public interface IRepository<T> where T : class
{
    Task<T> getById(int id);

    Task<T> getByType(string id);

    Task<T> Insert(T entity);

    Task<IList<T>> Insert(IEnumerable<T> entities);

    Task<T> Update(T entity);

    string Delete(int[] entitieIds);

    Task<IQueryable<T>> Fetch();

    Task<string> DeleteAsync(int Id);

    IQueryable<T> Table { get; }

    IQueryable<T> TableNoTracking { get; }

}


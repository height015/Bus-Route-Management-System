

using System.Linq.Expressions;
using Backend.Domain.Common;
using BRMSAPI.Data;
using Configuration;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;


public class Repository<T> : IRepository<T> where T : class
    {

    private readonly AppDbContext _appDbContext;


    public Repository(AppDbContext requestDbContext)
        {
            _appDbContext = requestDbContext;
        }
        public IQueryable<T> Table => _appDbContext.Set<T>();

        public IQueryable<T> TableNoTracking => _appDbContext.Set<T>().AsNoTracking();

        public async Task<string> DeleteAsync(int Id)
        {
            string message = string.Empty;
            try
            {
            var entity =  _appDbContext.Set<T>().Find(Id);
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));

                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    _appDbContext.Set<T>().Update(entity);
                    break;

                default:
                    _appDbContext.Set<T>().Remove(entity);
                    break;
            }

            _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                 ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            }
            return await Task.FromResult(message);

        }

        public string Delete(int[] entitieIds){

        string message = string.Empty;
        try
        {
            foreach (var itemId in entitieIds)
            {
                var entity = _appDbContext.Set<T>().Find(itemId);
                if (entity != null)
                {

                    if (entity is ISoftDeletedEntity softDeletedEntity)
                    {
                        softDeletedEntity.Deleted = true;
                        _appDbContext.Set<T>().Update(entity);
                    }
                    else
                    {
                        _appDbContext.Set<T>().Remove(entity);

                    }
                    _appDbContext.SaveChanges();
                }
            }
            return message;
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            return message = ex.Message;
        }
    }

        public async Task<IQueryable<T>> Fetch()
        {
        var retVal = _appDbContext.Set<T>();
            return await Task.FromResult(retVal);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _appDbContext.Set<T>().Where(expression);
        }

        public async Task<T> getById(int id)
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> getByType(string type)
        {
            return await _appDbContext.Set<T>().FindAsync(type);
        }



        public async Task<T> Insert(T entity)
        {
            await _appDbContext.Set<T>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IList<T>> Insert(IEnumerable<T> entities)
        {
            try
            {
                await _appDbContext.Set<T>().AddRangeAsync(entities);
                await _appDbContext.SaveChangesAsync();
                return entities.ToList();

            }
            catch (Exception ex)
            {

            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);

            return Enumerable.Empty<T>().ToList();
        }

        }
        public async Task<T> Update(T entity)
        {
            try
            {

            _appDbContext.Set<T>().Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
            }
            catch (Exception ex)
            {
                ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

       
    }

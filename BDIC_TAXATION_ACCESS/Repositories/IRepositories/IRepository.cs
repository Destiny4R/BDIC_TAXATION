using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        // Asynchronously retrieves an entity by its unique identifier.
        T GetByIdSingle(int id);

        // Asynchronously retrieves all entities.
        IEnumerable<T> GetAllWithoutCondition();

        // Asynchronously retrieves all entities that match the specified predicate, 
        // optionally including related entities and ordering the results.
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
                                         Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null,
                                         Expression<Func<T, object>>? orderBy = null,
                                         bool trackChanges = false);

        // Asynchronously finds entities that match the specified predicate, 
        // optionally including related entities and ordering the results.
        T FindSingle(Expression<Func<T, bool>>? predicate = null,
                                       Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null,
                                       Expression<Func<T, object>>? orderBy = null,
                                       bool trackChanges = false);

        // Asynchronously finds entities that match the specified predicate.
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);

        // Asynchronously retrieves an entity by its unique identifier, 
        // optionally including related entities and specifying whether to track changes.
        Task<T?> GetByConditionAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, bool trackChanges = true);

        // Asynchronously adds a new entity.
        Task AddAsync(T entity);
        void Add(T entity);
        // Asynchronously adds a range of new entities.
        Task AddRangeAsync(IEnumerable<T> entities);

        // Updates an existing entity.
        void Update(T entity);

        // Removes an existing entity.
        void Remove(T entity);

        // Removes a range of existing entities.
        void RemoveRange(IEnumerable<T> entities);
    }
}

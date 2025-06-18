using BDIC_TAXATION_ACCESS.Data;
using BDIC_TAXATION_ACCESS.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_ACCESS.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // Protected members for the context and DbSet
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        // Constructor to initialize the context and DbSet
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Method to get an entity by its ID asynchronously
        public T GetByIdSingle(int id)
        {
            return _dbSet.Find(id);
        }

        // Method to get all entities asynchronously
        public IEnumerable<T> GetAllWithoutCondition()
        {
            return _dbSet.ToList();
        }

        // Method to get all entities with optional filtering, including, ordering, and tracking options
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
        {
            IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

            if (includes != null)
            {
                query = includes(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return await query.ToListAsync();
        }

        // Method to get an entity by its ID with optional including and tracking options
        public async Task<T?> GetByConditionAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, bool trackChanges = true)
        {
            var query = trackChanges ? _dbSet.AsQueryable() : _dbSet.AsNoTracking();

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        // Method to find entities with optional filtering, including, ordering, and tracking options
        public T FindSingle(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
        {
            IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return query.FirstOrDefault();
        }

        // Method to find entities by a predicate
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
        {
            IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return await query.FirstOrDefaultAsync();
        }

        // Method to add a new entity asynchronously
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public void Add(T entity)
        {
             _dbSet.Add(entity);
        }
        // Method to add a range of new entities asynchronously
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        // Method to update an existing entity
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Method to remove an existing entity
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        // Method to remove a range of existing entities
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}

using Hospital_MS.Core._Data;
using Hospital_MS.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> baseQuery = _dbContext.Set<T>().AsNoTracking();

            if (filter != null)
            {
                baseQuery = baseQuery.Where(filter);
            }

            return baseQuery;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken) => await _dbContext.Set<T>().AddAsync(entity, cancellationToken);

        public void Update(T entity) => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => _dbContext.Set<T>().RemoveRange(entities);

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.Set<T>().CountAsync(predicate, cancellationToken);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Set<T>().CountAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken) => await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);



        // *** *** *** ***
        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
                => await _dbContext.Set<T>().FindAsync([id], cancellationToken);

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
                => await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

        public IQueryable<T> GetAllAsQueryable(CancellationToken cancellationToken)
                => _dbContext.Set<T>().AsQueryable();

    }
}

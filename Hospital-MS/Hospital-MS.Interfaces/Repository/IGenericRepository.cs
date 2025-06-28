using System.Linq.Expressions;

namespace Hospital_MS.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        void Update(T entity);
        void Delete(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        void DeleteRange(IEnumerable<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        // *** *** *** ***
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        IQueryable<T> GetAllAsQueryable(CancellationToken cancellationToken = default);
        Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);
    }
}

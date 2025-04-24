using Hospital_MS.Core.Specifications;
using System.Linq.Expressions;

namespace Hospital_MS.Interfaces.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T?> GetByIdWithIncludesAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(T spec, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllWithWithIncludesAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdWithSpecAsync(T spec, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default); // for Count all
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(T spec, CancellationToken cancellationToken = default);
        IQueryable<T> GetAllAsQueryable(T? spec);
        IQueryable<T> GetAllAsQueryable();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);


        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}

using Hospital_MS.Core._Data;
using Hospital_MS.Interfaces.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace Hospital_MS.Services.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private Hashtable _repositories;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }


        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var key = typeof(TEntity).Name; // ex: Order

            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);

                _repositories.Add(key, repository);
            }

            return _repositories[key] as IGenericRepository<TEntity>;
        }



        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
         => await _dbContext.SaveChangesAsync(cancellationToken);

        public async ValueTask DisposeAsync()
        => await _dbContext.DisposeAsync();

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}

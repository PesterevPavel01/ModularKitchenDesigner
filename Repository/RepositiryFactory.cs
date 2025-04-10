using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public sealed class RepositiryFactory<TContext> : IRepositoryFactory
         where TContext : DbContext
    {
        #region fields

        private Dictionary<Type, object>? _repositories;
        private bool _disposed;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositiryFactory{TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public RepositiryFactory(TContext context)
        {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region properties

        public TContext DbContext { get; }

        #endregion

        #region Methods

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            _repositories ??= [];

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new BaseRepository<TEntity>(DbContext);
            }

            return (IBaseRepository<TEntity>)_repositories[type];
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories?.Clear();
                    DbContext.Dispose();
                }
            }
            _disposed = true;
        }
    }
}

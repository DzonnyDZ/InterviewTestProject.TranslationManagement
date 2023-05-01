using Microsoft.EntityFrameworkCore;
using TranslationManagement.Data.Model;

namespace TranslationManagement.Data
{
    /// <summary>Base class for all repositories</summary>
    /// <typeparam name="TEntity">Type of main entity provided by the repository</typeparam>
    internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        /// <summary>Initializes a new instance of the <see cref="Repository{TEntity}"/> class.</summary>
        /// <param name="dbContext">Provides access to database</param>
        protected Repository(AppDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>Gets the context which provides access to the database</summary>
        protected AppDbContext DbContext { get; }

        /// <summary>Gets set of entities of type <typeparamref name="TEntity"/></summary>
        protected virtual DbSet<TEntity> Set => DbContext.Set<TEntity>();

        /// <summary>Gets all entities from database</summary>
        /// <returns>All entities from database, unfiltered</returns>
        public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync() => (IReadOnlyCollection<TEntity>)await Set.ToArrayAsync().ConfigureAwait(false);

        /// <summary>Gets entity identified by primary key</summary>
        /// <param name="id">ID (primary key) to get entity by</param>
        /// <returns>Entity identified by <paramref name="id"/>; null if no such entity exists</returns>
        public Task<TEntity?> GetByIdAsync(int id) => Set.Where(e => e.Id == id).FirstOrDefaultAsync();

        /// <summary>Adds a new entity to database</summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
        public async Task InsertAsync(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            await Set.AddAsync(entity).ConfigureAwait(false);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

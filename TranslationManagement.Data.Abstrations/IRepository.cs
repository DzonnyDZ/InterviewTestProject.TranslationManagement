using TranslationManagement.Data.Model;

namespace TranslationManagement.Data;

/// <summary>Base interfaces for data repositories</summary>
/// <typeparam name="TEntity">Type of main entity provided by the repository</typeparam>
public interface IRepository<TEntity> where TEntity : Entity
{
    /// <summary>Gets all entities from database</summary>
    /// <returns>All entities from database, unfiltered</returns>
    //TODO: This may be too much, some filtering or paging may be useful
    public Task<IReadOnlyCollection<TEntity>> GetAllAsync();

    /// <summary>Gets entity identified by primary key</summary>
    /// <param name="id">ID (primary key) to get entity by</param>
    /// <returns>Entity identified by <paramref name="id"/>; null if no such entity exists</returns>
    public Task<TEntity?> GetByIdAsync(int id);

    /// <summary>Adds a new entity to database</summary>
    /// <param name="entity">The entity to add. <paramref name="entity"/>.<see cref="Entity.Id">Id</see> will be set to newly generated entity ID.</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    Task InsertAsync(TEntity entity);
}
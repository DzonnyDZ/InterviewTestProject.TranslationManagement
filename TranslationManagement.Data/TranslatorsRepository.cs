using Microsoft.EntityFrameworkCore;
using TranslationManagement.Data.Model;

namespace TranslationManagement.Data;

/// <summary>Implements database access for <see cref="Translator"/> entity</summary>
internal class TranslatorsRepository : Repository<Translator>, ITranslatorsRepository
{
    /// <summary>Initializes a new instance of the <see cref="TranslatorsRepository"/> class.</summary>
    /// <param name="dbContext">Provides access to database</param>
    public TranslatorsRepository(AppDbContext dbContext) : base(dbContext) { }

    /// <summary>Get all translators with the same name</summary>
    /// <param name="name">Name to filter translators by</param>
    /// <returns>All translators with given name</returns>
    public async Task<IReadOnlyCollection<Translator>> GetAllByNameAsync(string name) =>
        await Set.Where(t => t.Name == name).ToArrayAsync().ConfigureAwait(false);

    /// <summary>Updates translator status</summary>
    /// <param name="translatorId">ID of translator to update status for</param>
    /// <param name="status">New status to set for translator identified by <paramref name="translatorId"/></param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    public async Task UpdateTranslatorStatusAsync(int translatorId, string status)
    {
        var translator = await GetByIdAsync(translatorId).ConfigureAwait(false) ??
            throw new EntityNotFoundException(typeof(Translator), translatorId);

        if (translator.Status != status)
        {
            translator.Status = status;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

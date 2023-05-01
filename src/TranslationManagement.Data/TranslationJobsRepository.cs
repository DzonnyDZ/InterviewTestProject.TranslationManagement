using TranslationManagement.Data.Model;

namespace TranslationManagement.Data;

/// <summary>Implements database access for <see cref="TranslationJob"/> entity</summary>
internal class TranslationJobsRepository : Repository<TranslationJob>, ITranslationJobsRepository
{
    /// <summary>Initializes a new instance of the <see cref="TranslationJobsRepository"/> class.</summary>
    /// <param name="dbContext">Provides access to database</param>
    public TranslationJobsRepository(AppDbContext dbContext) : base(dbContext) { }

    /// <summary>Sets translation job status</summary>
    /// <param name="jobId">ID of job to set status for</param>
    /// <param name="status">The new status</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    public async Task UpdateJobStatusAsync(int jobId, string status)
    {
        var job = await GetByIdAsync(jobId).ConfigureAwait(false) ??
            throw new EntityNotFoundException(typeof(TranslationJob), jobId);

        if (job.Status != status)
        {
            job.Status = status;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

using TranslationManagement.Data.Model;

namespace TranslationManagement.Data;

/// <summary>Implements database access for <see cref="TranslationJob"/> entity</summary>
internal class TranslationJobsRepository : Repository<TranslationJob>, ITranslationJobsRepository
{
    /// <summary>Initializes a new instance of the <see cref="TranslationJobsRepository"/> class.</summary>
    /// <param name="dbContext">Provides access to database</param>
    public TranslationJobsRepository(AppDbContext dbContext) : base(dbContext) { }

    /// <summary>Sets translator associated with a job</summary>
    /// <param name="jobId">ID of job to set translator for</param>
    /// <param name="translatorId">ID of translator to associate with the job</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    public async Task SetJobTranslatorAsync(int jobId, int translatorId)
    {
        var job = await GetByIdAsync(jobId).ConfigureAwait(false) ??
            throw new EntityNotFoundException(typeof(TranslationJob), jobId);

        if (job.TranslatorId == null || translatorId != job.TranslatorId.Value)
        {
            job.TranslatorId = translatorId;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    /// <summary>Sets translation job status</summary>
    /// <param name="jobId">ID of job to set status for</param>
    /// <param name="status">The new status</param>
    /// <param name="translatorId">If specified also updates translator ID</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    public async Task UpdateJobStatusAsync(int jobId, string status, int? translatorId = null)
    {
        var job = await GetByIdAsync(jobId).ConfigureAwait(false) ??
            throw new EntityNotFoundException(typeof(TranslationJob), jobId);

        if (job.Status != status || (translatorId.HasValue && (job.TranslatorId == null || translatorId.Value != job.TranslatorId.Value)))
        {
            job.Status = status;
            if (translatorId.HasValue) job.TranslatorId = translatorId.Value;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

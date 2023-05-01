using TranslationManagement.Data.Model;

namespace TranslationManagement.Data;

/// <summary>Provides database access for <see cref="TranslationJob"/> entity</summary>
public interface ITranslationJobsRepository : IRepository<TranslationJob>
{
    /// <summary>Sets translator associated with a job</summary>
    /// <param name="jobId">ID of job to set translator for</param>
    /// <param name="translatorId">ID of translator to associate with the job</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    Task SetJobTranslatorAsync(int jobId, int translatorId);

    /// <summary>Sets translation job status</summary>
    /// <param name="jobId">ID of job to set status for</param>
    /// <param name="status">The new status</param>
    /// <param name="translatorId">If specified also updates translator ID</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    Task UpdateJobStatusAsync(int jobId, string status, int? translatorId = null);
}

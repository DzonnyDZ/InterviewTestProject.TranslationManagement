using TranslationManagement.Data.Model;

namespace TranslationManagement.Data;

/// <summary>Provides database access for <see cref="TranslationJob"/> entity</summary>
public interface ITranslationJobsRepository : IRepository<TranslationJob>
{
    /// <summary>Sets translation job status</summary>
    /// <param name="jobId">ID of job to set status for</param>
    /// <param name="status">The new status</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    Task UpdateJobStatusAsync(int jobId, string status);
}

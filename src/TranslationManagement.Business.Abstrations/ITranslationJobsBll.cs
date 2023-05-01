using TranslationManagement.Dto;

namespace TranslationManagement.Business;

/// <summary>Business layer for working with translation jobs</summary>
public interface ITranslationJobsBll
{
    /// <summary>Gets all translators unconditionally</summary>
    /// <returns>All translators from database without filtering</returns>
    //TODO: This may be to much, some filtering or paging may be useful
    Task<IReadOnlyCollection<TranslationJobModel>> GetAllAsync();

    /// <summary>Saves a new translation job</summary>
    /// <param name="job">The translation job data to save. <paramref name="job"/>.<see cref="TranslationJobModel.Id">Id</see> will be set to database-generated ID.</param>
    /// <returns>ID of newly created job</returns>
    Task<int> CreateJobAsync(TranslationJobCreationModel job);

    /// <summary>Creates a new translation job from file</summary>
    /// <param name="fileStream">The file to create translation job from</param>
    /// <param name="fileMimeType">MIME type of data in <paramref name="fileStream"/></param>
    /// <param name="fileName">Original file name (as uploaded)</param>
    /// <param name="customer">Customer identification</param>
    /// <returns>The job created</returns>
    Task<TranslationJobModel> CreateJobWithFileAsync(Stream fileStream, string fileMimeType, string fileName, string customer);

    /// <summary>Updates translation job status</summary>
    /// <param name="jobId">ID of the job to update status of</param>
    /// <param name="translatorId">Identifies translator who's requesting the update</param>
    /// <param name="status">The new status</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    Task UpdateJobStatusAsync(int jobId, int translatorId, string status);

    /// <summary>Gets <see cref="TranslationJobModel"/> identified by ID</summary>
    /// <param name="id">ID of job to get</param>
    /// <returns>Object representing requested job, null if no job with given <paramref name="id"/> is in database.</returns>
    Task<TranslationJobModel> GetJobByIdAsync(int id);
}

using System.Reflection;
using AutoMapper;
using External.ThirdParty.Services;
using Microsoft.Extensions.Logging;
using TranslationManagement.Business.JobReaders;
using TranslationManagement.Data;
using TranslationManagement.Data.Model;
using TranslationManagement.Dto;
using TranslationManagement.Extensions;

namespace TranslationManagement.Business;

/// <summary>Implements business layer for working with translation jobs</summary>
internal class TranslationJobsBll : ITranslationJobsBll
{
    private readonly ITranslationJobsRepository repository;
    private readonly IMapper mapper;
    private readonly INotificationService notificationService;
    private readonly ILogger logger;
    private readonly IJobFileReaderFactory readerFactory;
    private const double PricePerCharacter = 0.01;

    /// <summary>Allowed job statuses determined from <see cref="JobStatus"/>'es properties</summary>
    private static readonly string[] JobStatuses =
        (from f in typeof(JobStatus).GetFields(BindingFlags.Public | BindingFlags.Static) select (string)f.GetValue(null)!).ToArray();

    /// <summary>Gets or sets initial delay (in milliseconds) to wait (before next call) when <see cref="INotificationService.SendNotification"/> call failes</summary>
    internal int ExponentialBackOffInitialDelay { get; set; } = 500;

    /// <summary>Initializes a new instance of the <see cref="TranslationJobsBll"/> class.</summary>
    /// <param name="repository">Provides access to <see cref="TranslationJob"/> database storage</param>
    /// <param name="notificationService">Sends notifications about jobs</param>
    /// <param name="mapper">Performs mapping from data model to DTO</param>
    /// <param name="readerFactory">Creates instances of <see cref="TxtJobReader"/> implementations</param>
    /// <param name="logger">Logging sink</param>
    public TranslationJobsBll(ITranslationJobsRepository repository, INotificationService notificationService, IMapper mapper, IJobFileReaderFactory readerFactory, ILogger<TranslationJobsBll> logger)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.readerFactory = readerFactory ?? throw new ArgumentNullException(nameof(readerFactory));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>Determines and sets job price</summary>
    /// <param name="job">The job to set price for</param>
    /// <remarks><paramref name="job"/>.<see cref="TranslationJobModel.Price">Price</see> is set.</remarks>
    private static void SetPrice(TranslationJob job) => job.Price = job.OriginalContent.Length * PricePerCharacter;

    /// <summary>Gets all translators unconditionally</summary>
    /// <returns>All translators from database without filtering</returns>
    public async Task<IReadOnlyCollection<TranslationJobModel>> GetAllAsync() =>
        mapper.MapAll<TranslationJobModel>(await repository.GetAllAsync().ConfigureAwait(false));

    /// <summary>Saves a new translation job</summary>
    /// <param name="job">The translation job data to save. <paramref name="job"/>.<see cref="TranslationJobModel.Id">Id</see> will be set to database-generated ID.</param>
    /// <returns>ID of the job created</returns>
    public async Task<int> CreateJobAsync(TranslationJobCreationModel job)
    {
        TranslationJob entity = mapper.Map<TranslationJob>(job);
        entity.Status = JobStatus.New;
        SetPrice(entity);
        await repository.InsertAsync(entity).ConfigureAwait(false);
        await NotifyOnNewJobAsync(entity).ConfigureAwait(false);
        return entity.Id;
    }

    /// <summary>Sends notification via notification service that a new job has been created</summary>
    /// <param name="entity">The job that has been created</param>
    /// <returns>True if notification was sent successfully; false otherwise</returns>
    internal async Task<bool> NotifyOnNewJobAsync(TranslationJob entity)
    {
        bool success;
        int attempt = 0;
        do
        {
            if (attempt++ > 0) await Task.Delay(TimeSpan.FromMilliseconds(ExponentialBackOffInitialDelay * Math.Pow(2, attempt - 1))).ConfigureAwait(false); //Exponential back-off; base ExponentialBackOffInitialDelay, factor 2

            try
            {
                success = await notificationService.SendNotification("Job created: " + entity.Id).ConfigureAwait(false);
            }
            catch (ApplicationException ex)
            {
                logger.LogError(ex, "Notification service failure: {message}", ex.Message);
                success = false;
            }
        }
        while (!success && attempt < 5);

        if (success)
            logger.LogInformation("New job notification sent");
        else
            logger.LogError("Failed to send new job notification");

        return success;
    }

    /// <summary>Creates a new translation job from file</summary>
    /// <param name="fileStream">The file to create translation job from</param>
    /// <param name="fileMimeType">MIME type of data in <paramref name="fileStream"/></param>
    /// <param name="fileName">Original file name (as uploaded)</param>
    /// <param name="customer">Customer identification</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    public async Task<TranslationJobModel> CreateJobWithFileAsync(Stream fileStream, string fileMimeType, string fileName, string customer)
    {
        var fileReader = readerFactory.CreateReader(fileMimeType, fileName);

        var (content, customer2) = await fileReader.ReadAsync(fileStream).ConfigureAwait(false);
        if (customer2 is not null) customer = customer2;

        var job = new TranslationJobCreationModel()
        {
            OriginalContent = content,
            CustomerName = customer,
        };

        int jobId = await CreateJobAsync(job).ConfigureAwait(false);
        return mapper.Map<TranslationJobModel>(await repository.GetByIdAsync(jobId).ConfigureAwait(false));
    }

    /// <summary>Updates translation job status</summary>
    /// <param name="jobId">ID of the job to update status of</param>
    /// <param name="translatorId">Identifies translator who's requesting the update</param>
    /// <param name="status">The new status</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    public async Task UpdateJobStatusAsync(int jobId, int translatorId, string status)
    {
        logger.LogInformation("Job status update request received: {status} for job {jobId} by translator {translatorId}", status, jobId, translatorId);
        if (JobStatuses.All(s => s != status))
            throw new ArgumentException($"Invalid job status '{status}'", nameof(status));

        var job = await repository.GetByIdAsync(jobId).ConfigureAwait(false);
        if (job == null) throw new EntityNotFoundException(typeof(TranslationJob), jobId);

        if (job.Status == status) return;

        bool isInvalidStatusChange = (job.Status == JobStatus.New && status == JobStatus.Completed) ||
                                     job.Status == JobStatus.Completed || status == JobStatus.New;
        if (isInvalidStatusChange)
            throw new InvalidOperationException($"Invalid job status transition from '{job.Status}' to '{status}'");

        await repository.UpdateJobStatusAsync(jobId, status).ConfigureAwait(false);
    }

    /// <summary>Gets <see cref="TranslationJobModel"/> identified by ID</summary>
    /// <param name="id">ID of job to get</param>
    /// <returns>Object representing requested job, null if no job with given <paramref name="id"/> is in database.</returns>
    public async Task<TranslationJobModel> GetJobByIdAsync(int id) => mapper.Map<TranslationJobModel>(await repository.GetByIdAsync(id).ConfigureAwait(false));

    /// <summary>Defines constants of possible job statuses</summary>
    internal static class JobStatus
    {
        /// <summary>New translation job (translation didn't start yet)</summary>
        public const string New = nameof(New);

        /// <summary>The translation is in progress</summary>
        public const string InProgress = nameof(InProgress);

        /// <summary>The translation is completed</summary>
        public const string Completed = nameof(Completed);
    }
}

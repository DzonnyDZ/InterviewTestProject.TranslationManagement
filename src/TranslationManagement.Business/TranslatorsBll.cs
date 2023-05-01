using AutoMapper;
using TranslationManagement.Data;
using TranslationManagement.Data.Model;
using TranslationManagement.Dto;
using TranslationManagement.Extensions;

namespace TranslationManagement.Business;

/// <summary>Implements business layer for working with Translators</summary>
internal class TranslatorsBll : ITranslatorsBll
{
    private readonly ITranslatorsRepository repository;
    private readonly IMapper mapper;
    private static readonly string[] TranslatorStatuses = { "Applicant", "Certified", "Deleted" };

    /// <summary>Initializes a new instance of the <see cref="TranslatorsBll"/> class</summary>
    /// <param name="repository">Provides access to translators database</param>
    /// <param name="mapper">Performs mapping between data model and DTO</param>
    public TranslatorsBll(ITranslatorsRepository repository, IMapper mapper)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>Gets all translators unconditionally</summary>
    /// <returns>All translators from database without filtering</returns>
    public async Task<IReadOnlyCollection<TranslatorModel>> GetAllAsync() =>
        mapper.MapAll<TranslatorModel>(await repository.GetAllAsync().ConfigureAwait(false));

    /// <summary>Gets all translators with same name</summary>
    /// <param name="name">Name of the translator</param>
    /// <returns>All translators with same name</returns>
    public async Task<IReadOnlyCollection<TranslatorModel>> GetByNameAsync(string name) =>
        mapper.MapAll<TranslatorModel>(await repository.GetAllByNameAsync(name).ConfigureAwait(false));

    /// <summary>Saves a new translator to database</summary>
    /// <param name="translator">Translator data to save. <paramref name="translator"/>.<see cref="TranslatorModel.Id">Id</see> will be set to database-generated ID.</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    public async Task AddTranslatorAsync(TranslatorModel translator)
    {
        if (translator is null) throw new ArgumentNullException(nameof(translator));
        if (TranslatorStatuses.All(s => translator.Status != s))
            throw new ArgumentException($"Unknown translator status '{translator.Status}'", nameof(translator));
        Translator entity = mapper.Map<Translator>(translator);
        await repository.InsertAsync(entity).ConfigureAwait(false);
        translator.Id = entity.Id;
    }

    /// <summary>Updates translator status</summary>
    /// <param name="translatorId">ID of translator to update status</param>
    /// <param name="status">The status to set</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    public async Task SetStatusAsync(int translatorId, string status)
    {
        if (TranslatorStatuses.All(s => status != s))
            throw new ArgumentException($"Unknown translator status '{status}'", nameof(status));

        await repository.UpdateTranslatorStatusAsync(translatorId, status).ConfigureAwait(false);
    }

    /// <summary>Gets <see cref="TranslatorModel"/> identified by ID</summary>
    /// <param name="id">ID of translator to get</param>
    /// <returns>Object representing requested translator, null if no translator with given <paramref name="id"/> is in database.</returns>
    public async Task<TranslatorModel?> GetTranslatorByIdAsync(int id) => mapper.Map<TranslatorModel>(await repository.GetByIdAsync(id).ConfigureAwait(false));
}

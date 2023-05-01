using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Business;
using TranslationManagement.Dto;

namespace TranslationManagement.Api.Controllers;

/// <summary>Allows to manage translators</summary>
[ApiController]
[Route("api/translators")]
public class TranslatorManagementController : ControllerBase
{
    private readonly ITranslatorsBll businessLayer;
    private readonly ILogger<TranslatorManagementController> logger;

    /// <summary>Initializes a new instance of the <see cref="TranslatorManagementController"/> class.</summary>
    /// <param name="businessLayer">Translators business layer</param>
    /// <param name="logger">Logging sink</param>
    public TranslatorManagementController(ITranslatorsBll businessLayer, ILogger<TranslatorManagementController> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.businessLayer = businessLayer ?? throw new ArgumentNullException(nameof(businessLayer));
    }

    /// <summary>Gets translators (optionally filtered by name)</summary>
    /// <param name="name">The name to filter translators by</param>
    /// <returns>All translators sharing the same name</returns>
    //TODO: This may be too much and some filtering or paging would be useful (when name is null)
    [HttpGet]
    public Task<IReadOnlyCollection<TranslatorModel>> GetTranslatorsByName(string name = null) =>
        string.IsNullOrEmpty(name) ? businessLayer.GetAllAsync() : businessLayer.GetByNameAsync(name);

    /// <summary>Saves translator to database</summary>
    /// <param name="translator">Translator data to save</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    /// <response code="201">The translator has been created</response>
    [HttpPost]
    [ProducesResponseType(typeof(TranslatorModel), 201)]
    public async Task<IActionResult> AddTranslator(TranslatorModel translator)
    {
        if (translator is null) throw new ArgumentNullException(nameof(translator));
        await businessLayer.AddTranslatorAsync(translator);
        translator = await businessLayer.GetTranslatorByIdAsync(translator.Id);
        return new CreatedResult($"api/translators/{translator.Id}", translator);
    }

    /// <summary>Updates translator status</summary>
    /// <param name="translator">ID of translator to update status of</param>
    /// <param name="newStatus">The new translator status</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    /// <response code="204">The status has been changed</response>
    [HttpPut($"{{{nameof(translator)}}}/status")]
    [ProducesResponseType(typeof(void), 204)]
    public async Task<IActionResult> UpdateTranslatorStatus(int translator, string newStatus)
    {
        logger.LogInformation("User status update request: {newStatus} for user {translator}", newStatus, translator);
        await businessLayer.SetStatusAsync(translator, newStatus);
        return NoContent();
    }
}
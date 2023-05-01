using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranslationManagement.Business;
using TranslationManagement.Dto;

namespace TranslationManagement.Api.Controllers;

/// <summary>Allows to manage translation jobs</summary>
[ApiController]
[Route("api/jobs")]
public class TranslationJobController : ControllerBase
{
    private readonly ITranslationJobsBll businessLayer;

    /// <summary>Initializes a new instance of the <see cref="TranslationJobController"/> class.</summary>
    /// <param name="businessLayer">Translation jobs business layer</param>
    public TranslationJobController(ITranslationJobsBll businessLayer)
    {
        this.businessLayer = businessLayer ?? throw new ArgumentNullException(nameof(businessLayer));
    }

    /// <summary>Gets all translation jobs</summary>
    /// <returns>All translation jobs</returns>
    //TODO: This may be too much and some filtering or paging would be useful
    [HttpGet]
    public Task<IReadOnlyCollection<TranslationJobModel>> GetJobs() => businessLayer.GetAllAsync();

    /// <summary>Saves a new translation job</summary>
    /// <param name="job">The new job to save</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    /// <response code="201">The job has been created</response>
    [HttpPost]
    [ProducesResponseType(typeof(TranslationJobModel), 201)]
    public async Task<IActionResult> CreateJob(TranslationJobCreationModel job)
    {
        int jobId = await businessLayer.CreateJobAsync(job);
        job = await businessLayer.GetJobByIdAsync(jobId);
        return new CreatedResult($"api/jobs/{jobId}", job);
    }

    /// <summary>Saves a new translation job specified by file</summary>
    /// <param name="file">The file uploaded</param>
    /// <param name="customer">Customer identification</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    /// <response code="201">The job has been created</response>
    [HttpPost("file")]
    [ProducesResponseType(typeof(TranslationJobModel), 201)]
    public async Task<IActionResult> CreateJobWithFile(IFormFile file, string customer)
    {
        using var stream = file.OpenReadStream();
        var job = await businessLayer.CreateJobWithFileAsync(stream, file.ContentType, file.FileName, customer);
        return new CreatedResult($"api/jobs/{job.Id}", job);
    }

    /// <summary>Updates translation job status</summary>
    /// <param name="jobId">ID of job to update status for</param>
    /// <param name="translatorId">ID of translator who's requesting the update</param>
    /// <param name="newStatus">New status to save</param>
    /// <returns>Task to await to wait for the asynchronous operation to complete</returns>
    /// <response code="204">The status has been changed</response>
    [HttpPut($"{{{nameof(jobId)}}}/status")]
    [ProducesResponseType(typeof(void), 204)]
    public async Task<IActionResult> UpdateJobStatus(int jobId, int translatorId, string newStatus = "")
    {
        await businessLayer.UpdateJobStatusAsync(jobId, translatorId, newStatus);
        return NoContent();
    }
}
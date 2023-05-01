#nullable disable

namespace TranslationManagement.Dto;

/// <summary>Represents a translation job - a task to translate text from one language to another</summary>
public class TranslationJobModel : TranslationJobCreationModel
{
    /// <summary>Gets or sets entity primary key</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets job status</summary>
    public string Status { get; set; }
    /// <summary>Gets or sets the translated text</summary>
    public string TranslatedContent { get; set; }
    /// <summary>Gets or sets job price</summary>
    public double Price { get; set; }

    /// <summary>Gets or sets ID of translator who works or worked on the job</summary>
    public int? TranslatorId { get; set; }
}

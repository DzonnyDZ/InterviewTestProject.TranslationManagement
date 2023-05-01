#nullable disable

namespace TranslationManagement.Dto;

/// <summary>Subset of data of translation job necessary to create a new translation job</summary>
public class TranslationJobCreationModel
{
    /// <summary>Gets or sets customer identification</summary>
    public string CustomerName { get; set; }
    /// <summary>Gets or sets the text to be translated</summary>
    public string OriginalContent { get; set; }
}
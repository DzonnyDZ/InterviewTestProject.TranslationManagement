#nullable disable

namespace TranslationManagement.Data.Model;

/// <summary>Represents a translation job - a task to translate text from one language to another</summary>
public class TranslationJob : Entity
{
    /// <summary>Gets or sets customer identification</summary>
    public string CustomerName { get; set; }
    /// <summary>Gets or sets job status</summary>
    public string Status { get; set; }
    /// <summary>Gets or sets the text to be translated</summary>
    public string OriginalContent { get; set; }
    /// <summary>Gets or sets the translated text</summary>
    public string TranslatedContent { get; set; }
    /// <summary>Gets or sets job price</summary>
    public double Price { get; set; }
}

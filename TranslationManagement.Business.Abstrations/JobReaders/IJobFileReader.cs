namespace TranslationManagement.Business.JobReaders;

/// <summary>Reads job specification from file</summary>
public interface IJobFileReader
{
    /// <summary>Reads the file content</summary>
    /// <param name="stream">The stream to read file content from</param>
    /// <returns>Data read from file content: The content to translate and optionally customer identification</returns>
    Task<(string content, string? customer)> ReadAsync(Stream stream);
}

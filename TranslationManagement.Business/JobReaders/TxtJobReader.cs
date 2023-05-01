namespace TranslationManagement.Business.JobReaders;

/// <summary>Reads job specification from plain text (TXT) file</summary>
internal class TxtJobReader : IJobFileReader
{
    /// <summary>Reads the file content</summary>
    /// <param name="stream">The stream to read file content from</param>
    /// <returns>Data read from file content: The content to translate; this implementation does not provide customer identification</returns>
    public async Task<(string content, string? customer)> ReadAsync(Stream stream)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        using var reader = new StreamReader(stream);
        return (await reader.ReadToEndAsync().ConfigureAwait(false), null);
    }
}

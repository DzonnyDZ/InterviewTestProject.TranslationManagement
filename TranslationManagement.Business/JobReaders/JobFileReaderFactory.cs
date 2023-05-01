namespace TranslationManagement.Business.JobReaders;

/// <summary>Creates instance of <see cref="IJobFileReader"/> implementations suitable for reading various file types</summary>
internal class JobFileReaderFactory : IJobFileReaderFactory
{
    /// <summary>Creates an instance of <see cref="IJobFileReader"/> implementation based on file MIME type or extension</summary>
    /// <param name="mimeType">File MIME type</param>
    /// <param name="fileName">
    /// File name (as uploaded;
    /// extension of the file name is considered to determine file type if <paramref name="mimeType"/> is <c>application/octet-stream</c>).
    /// </param>
    /// <returns>A new instance of <see cref="IJobFileReader"/> implementation suitable for reading files of type determine from <paramref name="mimeType"/> or extension</returns>
    /// <exception cref="NotSupportedException">No suitable implementation for reading file of type identified by <paramref name="mimeType"/> or <paramref name="fileName"/> extension.</exception>
    public IJobFileReader CreateReader(string mimeType, string fileName)
    {
        switch (mimeType)
        {
            case "text/plain": return new TxtJobReader();
            case "application/xml" or "text/xml": return new XmlJobReader();
            case "application/octet-stream":
                return Path.GetExtension(fileName).ToLowerInvariant() switch
                {
                    ".txt" => new TxtJobReader(),
                    ".xml" => new XmlJobReader(),
                    _ => throw new NotSupportedException($"Unsupported file extension '{Path.GetExtension(fileName)}' for MIME type 'application/octet-stream'."),
                };
            default: throw new NotSupportedException($"Unsupported MIME type '{mimeType}'");
        }
    }
}

namespace TranslationManagement.Business.JobReaders;

/// <summary>Creates instance of <see cref="IJobFileReader"/> implementations suitable for reading various file types</summary>
public interface IJobFileReaderFactory
{
    /// <summary>Creates an instance of <see cref="IJobFileReader"/> implementation based on file MIME type or extension</summary>
    /// <param name="mimeType">File MIME type</param>
    /// <param name="fileName">
    /// File name (as uploaded;
    /// extension of the file name is considered to determine file type if <paramref name="mimeType"/> is <c>application/octet-stream</c>).
    /// </param>
    /// <returns>A new instance of <see cref="IJobFileReader"/> implementation suitable for reading files of type determine from <paramref name="mimeType"/> or extension</returns>
    /// <exception cref="NotSupportedException">No suitable implementation for reading file of type identified by <paramref name="mimeType"/> or <paramref name="fileName"/> extension.</exception>
    IJobFileReader CreateReader(string mimeType, string fileName);
}

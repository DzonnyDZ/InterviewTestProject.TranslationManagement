using System.Xml;
using System.Xml.Linq;

namespace TranslationManagement.Business.JobReaders;

/// <summary>Reads job specification from XML file</summary>
internal class XmlJobReader : IJobFileReader
{
    /// <summary>Reads the file content</summary>
    /// <param name="stream">The stream to read file content from</param>
    /// <returns>Data read from file content: The content to translate and optionally customer identification</returns>
    public async Task<(string content, string? customer)> ReadAsync(Stream stream)
    {
        var xdoc = await XDocument.LoadAsync(stream, LoadOptions.None, default).ConfigureAwait(false);
        if (xdoc is null) throw new XmlException("XML document not loaded, XML does not represent document");
        if (xdoc.Root is null) throw new XmlException("XML document does not have root element");
        string content = xdoc.Root.Element("Content")?.Value ?? throw new InvalidDataException("XML document does not contain job Content (<Content> element)");
        string? customer = xdoc.Root.Element("Customer")?.Value.Trim();
        return (content, customer);
    }
}

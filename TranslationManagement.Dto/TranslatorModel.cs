#nullable disable

namespace TranslationManagement.Dto;

/// <summary>Represents translator - a person who translates text from one language to another</summary>
public class TranslatorModel
{
    /// <summary>Gets or sets entity primary key</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets translator name</summary>
    public string Name { get; set; }
    /// <summary>Gets or sets translators hourly wage</summary>
    //TODO: Hourly rate would make more sense as number
    public string HourlyRate { get; set; }
    /// <summary>Gets or sets translator status</summary>
    public string Status { get; set; }
    /// <summary>Gets or sets translator credit card number</summary>
    public string CreditCardNumber { get; set; }
}

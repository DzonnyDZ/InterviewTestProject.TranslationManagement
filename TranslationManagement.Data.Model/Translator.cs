#nullable disable

namespace TranslationManagement.Data.Model;

/// <summary>Represents translator - a person who translates text from one language to another</summary>
public class Translator : Entity
{
    /// <summary>Gets or sets translator name</summary>
    public string Name { get; set; }
    /// <summary>Gets or sets translators hourly wage</summary>
    //TODO: Hourly rate would make more sense as number
    public string HourlyRate { get; set; }
    /// <summary>Gets or sets translator status</summary>
    public string Status { get; set; }
    /// <summary>Gets or sets translator credit card number</summary>
    //TODO: Credit card number should not be stored in database as plain text, either somehow encrypt it or store it in some safer storage (like key vault)
    public string CreditCardNumber { get; set; }
}

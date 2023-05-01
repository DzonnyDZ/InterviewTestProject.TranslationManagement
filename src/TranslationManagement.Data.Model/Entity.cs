namespace TranslationManagement.Data.Model;

/// <summary>Common base class for database entities</summary>
public abstract class Entity
{
    /// <summary>Gets or sets entity primary key</summary>
    public int Id { get; set; }
}

using Microsoft.EntityFrameworkCore;
using TranslationManagement.Data.Model;

namespace TranslationManagement.Data;

/// <summary>The database context</summary>
public class AppDbContext : DbContext
{
    /// <summary>Initializes a new instance of the <see cref="AppDbContext"/> class.</summary>
    /// <param name="options">The options for this context.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>Gets or sets as set that provides access to <see cref="TranslationJob"/>s</summary>
    public DbSet<TranslationJob> TranslationJobs { get; set; }

    /// <summary>Gets or sets a set that provides access to <see cref="Translator"/>s</summary>
    public DbSet<Translator> Translators { get; set; }
}
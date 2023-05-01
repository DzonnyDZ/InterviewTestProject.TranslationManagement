using Microsoft.EntityFrameworkCore.Migrations;

namespace TranslationManagement.Api.Migrations;

/// <summary>Initial database creation</summary>
public partial class InitialCreate : Migration
{
    /// <summary>Builds the operations that will migrate the database 'up'.</summary>
    /// <param name="migrationBuilder">The <see cref="MigrationBuilder"/> that will build the operations.</param>
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "TranslationJobs",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                CustomerName = table.Column<string>(type: "TEXT", nullable: true),
                Status = table.Column<string>(type: "TEXT", nullable: true),
                OriginalContent = table.Column<string>(type: "TEXT", nullable: true),
                TranslatedContent = table.Column<string>(type: "TEXT", nullable: true),
                Price = table.Column<double>(type: "REAL", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TranslationJobs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Translators",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: true),
                HourlyRate = table.Column<string>(type: "TEXT", nullable: true),
                Status = table.Column<string>(type: "TEXT", nullable: true),
                CreditCardNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Translators", x => x.Id);
            });
    }

    /// <summary>Builds the operations that will migrate the database 'down'.</summary>
    /// <param name="migrationBuilder">The <see cref="MigrationBuilder"/> that will build the operations.</param>
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "TranslationJobs");

        migrationBuilder.DropTable(
            name: "Translators");
    }
}

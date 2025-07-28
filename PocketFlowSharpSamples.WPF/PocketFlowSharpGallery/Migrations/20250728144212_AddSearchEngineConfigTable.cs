using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketFlowSharpGallery.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchEngineConfigTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchEngineConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EndPoint = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ApiKey = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchEngineConfigs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchEngineConfigs");
        }
    }
}

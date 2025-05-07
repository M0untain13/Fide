using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fide.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalysisFieldAndRenameUrlField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "ImageLinks",
                newName: "OriginalName");

            migrationBuilder.AddColumn<string>(
                name: "AnalysisName",
                table: "ImageLinks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalysisName",
                table: "ImageLinks");

            migrationBuilder.RenameColumn(
                name: "OriginalName",
                table: "ImageLinks",
                newName: "Url");
        }
    }
}

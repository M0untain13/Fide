using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fide.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsForResultAndDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AnalysisRequested",
                table: "ImageLinks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AnalysisResult",
                table: "ImageLinks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ImageLinks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalysisRequested",
                table: "ImageLinks");

            migrationBuilder.DropColumn(
                name: "AnalysisResult",
                table: "ImageLinks");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ImageLinks");
        }
    }
}

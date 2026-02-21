using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimescaleApi.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.RenameTable(
                name: "Results",
                newName: "results");

            migrationBuilder.AddPrimaryKey(
                name: "PK_results",
                table: "results",
                column: "FileName");

            migrationBuilder.CreateTable(
                name: "values",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValueIndicator = table.Column<double>(type: "double precision", nullable: false),
                    ExecutionTime = table.Column<double>(type: "double precision", nullable: false),
                    MetricValue = table.Column<double>(type: "double precision", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_values", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_values_FileName_Date",
                table: "values",
                columns: new[] { "FileName", "Date" },
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "values");

            migrationBuilder.DropPrimaryKey(
                name: "PK_results",
                table: "results");

            migrationBuilder.RenameTable(
                name: "results",
                newName: "Results");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                column: "FileName");

            migrationBuilder.CreateTable(
                name: "DataPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExecutionTime = table.Column<double>(type: "double precision", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MetricValue = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPoints", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataPoints_FileName_Date",
                table: "DataPoints",
                columns: new[] { "FileName", "Date" },
                descending: new bool[0]);
        }
    }
}

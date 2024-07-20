using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevProdWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ToolMetricTbl_ErrorCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToolMetricValue_ToolMetric_ToolMetricId",
                table: "ToolMetricValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToolMetricValue",
                table: "ToolMetricValue");

            migrationBuilder.RenameTable(
                name: "ToolMetricValue",
                newName: "ToolMetricValues");

            migrationBuilder.RenameIndex(
                name: "IX_ToolMetricValue_ToolMetricId",
                table: "ToolMetricValues",
                newName: "IX_ToolMetricValues_ToolMetricId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToolMetricValues",
                table: "ToolMetricValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMetricValues_ToolMetric_ToolMetricId",
                table: "ToolMetricValues",
                column: "ToolMetricId",
                principalTable: "ToolMetric",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToolMetricValues_ToolMetric_ToolMetricId",
                table: "ToolMetricValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToolMetricValues",
                table: "ToolMetricValues");

            migrationBuilder.RenameTable(
                name: "ToolMetricValues",
                newName: "ToolMetricValue");

            migrationBuilder.RenameIndex(
                name: "IX_ToolMetricValues_ToolMetricId",
                table: "ToolMetricValue",
                newName: "IX_ToolMetricValue_ToolMetricId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToolMetricValue",
                table: "ToolMetricValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMetricValue_ToolMetric_ToolMetricId",
                table: "ToolMetricValue",
                column: "ToolMetricId",
                principalTable: "ToolMetric",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

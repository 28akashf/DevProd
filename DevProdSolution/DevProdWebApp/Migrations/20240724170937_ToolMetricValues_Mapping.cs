using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevProdWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ToolMetricValues_Mapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Developers_Projects_ProjectId",
                table: "Developers");

            migrationBuilder.DropForeignKey(
                name: "FK_Metrics_Projects_ProjectId",
                table: "Metrics");

            migrationBuilder.DropIndex(
                name: "IX_Metrics_ProjectId",
                table: "Metrics");

            migrationBuilder.DropIndex(
                name: "IX_Developers_ProjectId",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Developers");

            migrationBuilder.AddColumn<int>(
                name: "DeveloperId",
                table: "ToolMetricValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ToolMetricValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ToolMetricValues_DeveloperId",
                table: "ToolMetricValues",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolMetricValues_ProjectId",
                table: "ToolMetricValues",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMetricValues_Developers_DeveloperId",
                table: "ToolMetricValues",
                column: "DeveloperId",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMetricValues_Projects_ProjectId",
                table: "ToolMetricValues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToolMetricValues_Developers_DeveloperId",
                table: "ToolMetricValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolMetricValues_Projects_ProjectId",
                table: "ToolMetricValues");

            migrationBuilder.DropIndex(
                name: "IX_ToolMetricValues_DeveloperId",
                table: "ToolMetricValues");

            migrationBuilder.DropIndex(
                name: "IX_ToolMetricValues_ProjectId",
                table: "ToolMetricValues");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "ToolMetricValues");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ToolMetricValues");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Metrics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Developers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_ProjectId",
                table: "Metrics",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Developers_ProjectId",
                table: "Developers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Developers_Projects_ProjectId",
                table: "Developers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Metrics_Projects_ProjectId",
                table: "Metrics",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}

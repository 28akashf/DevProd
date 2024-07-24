using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevProdWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ToolMetricValues_MappingNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToolMetricValues_Developers_DeveloperId",
                table: "ToolMetricValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolMetricValues_Projects_ProjectId",
                table: "ToolMetricValues");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ToolMetricValues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DeveloperId",
                table: "ToolMetricValues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMetricValues_Developers_DeveloperId",
                table: "ToolMetricValues",
                column: "DeveloperId",
                principalTable: "Developers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMetricValues_Projects_ProjectId",
                table: "ToolMetricValues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
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

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ToolMetricValues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeveloperId",
                table: "ToolMetricValues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}

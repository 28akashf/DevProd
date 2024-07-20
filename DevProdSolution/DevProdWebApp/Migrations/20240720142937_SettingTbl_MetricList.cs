using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevProdWebApp.Migrations
{
    /// <inheritdoc />
    public partial class SettingTbl_MetricList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SettingId",
                table: "ToolMetric",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToolMetric_SettingId",
                table: "ToolMetric",
                column: "SettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMetric_Settings_SettingId",
                table: "ToolMetric",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToolMetric_Settings_SettingId",
                table: "ToolMetric");

            migrationBuilder.DropIndex(
                name: "IX_ToolMetric_SettingId",
                table: "ToolMetric");

            migrationBuilder.DropColumn(
                name: "SettingId",
                table: "ToolMetric");
        }
    }
}

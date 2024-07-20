using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevProdWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ToolMetricTblSetting_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToolMetric_Settings_SettingId",
                table: "ToolMetric");

            migrationBuilder.AlterColumn<int>(
                name: "SettingId",
                table: "ToolMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMetric_Settings_SettingId",
                table: "ToolMetric",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToolMetric_Settings_SettingId",
                table: "ToolMetric");

            migrationBuilder.AlterColumn<int>(
                name: "SettingId",
                table: "ToolMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolMetric_Settings_SettingId",
                table: "ToolMetric",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id");
        }
    }
}

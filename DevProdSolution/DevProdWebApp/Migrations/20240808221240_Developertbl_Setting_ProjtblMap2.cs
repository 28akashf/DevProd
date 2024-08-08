using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevProdWebApp.Migrations
{
    /// <inheritdoc />
    public partial class Developertbl_Setting_ProjtblMap2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SettingId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SettingId",
                table: "Developers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SettingId",
                table: "Projects",
                column: "SettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Developers_SettingId",
                table: "Developers",
                column: "SettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Developers_Settings_SettingId",
                table: "Developers",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Settings_SettingId",
                table: "Projects",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Developers_Settings_SettingId",
                table: "Developers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Settings_SettingId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_SettingId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Developers_SettingId",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "SettingId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SettingId",
                table: "Developers");
        }
    }
}

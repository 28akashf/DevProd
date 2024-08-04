using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevProdWebApp.Migrations
{
    /// <inheritdoc />
    public partial class GlobalConfig_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalConfigs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalConfigs");
        }
    }
}

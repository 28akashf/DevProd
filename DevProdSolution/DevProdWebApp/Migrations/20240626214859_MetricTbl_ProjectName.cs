using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevProdWebApp.Migrations
{
    /// <inheritdoc />
    public partial class MetricTbl_ProjectName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Metrics",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Metrics");
        }
    }
}

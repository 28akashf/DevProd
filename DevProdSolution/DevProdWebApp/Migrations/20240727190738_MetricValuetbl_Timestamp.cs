using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevProdWebApp.Migrations
{
    /// <inheritdoc />
    public partial class MetricValuetbl_Timestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "ToolMetricValues",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "ToolMetricValues");
        }
    }
}

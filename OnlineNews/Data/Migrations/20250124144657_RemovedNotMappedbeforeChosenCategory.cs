using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineNews.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNotMappedbeforeChosenCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChosenCategory",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChosenCategory",
                table: "Articles");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HHPW_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class addedReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Review",
                table: "Orders");
        }
    }
}

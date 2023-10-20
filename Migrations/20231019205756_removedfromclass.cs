using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HHPW_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class removedfromclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Review",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Tip",
                table: "Orders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tip",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

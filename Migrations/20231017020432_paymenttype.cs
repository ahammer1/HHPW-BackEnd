using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HHPW_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class paymenttype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdersPaymentType",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "integer", nullable: false),
                    PaymentTypesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersPaymentType", x => new { x.OrdersId, x.PaymentTypesId });
                    table.ForeignKey(
                        name: "FK_OrdersPaymentType_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersPaymentType_PaymentType_PaymentTypesId",
                        column: x => x.PaymentTypesId,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdersPaymentType_PaymentTypesId",
                table: "OrdersPaymentType",
                column: "PaymentTypesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersPaymentType");
        }
    }
}

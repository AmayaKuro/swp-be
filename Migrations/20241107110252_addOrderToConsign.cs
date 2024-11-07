using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class addOrderToConsign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Consignments_OrderID",
                table: "Consignments",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Consignments_Orders_OrderID",
                table: "Consignments",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consignments_Orders_OrderID",
                table: "Consignments");

            migrationBuilder.DropIndex(
                name: "IX_Consignments_OrderID",
                table: "Consignments");
        }
    }
}

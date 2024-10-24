using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class updateConsignmentConstrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Orders_OrderID",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Orders_OrderID",
                table: "Transactions",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Orders_OrderID",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Orders_OrderID",
                table: "Transactions",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }
    }
}

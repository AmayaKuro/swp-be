using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class addConsignmentToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FosterConsignmentID",
                table: "Transactions",
                column: "FosterConsignmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Consignments_FosterConsignmentID",
                table: "Transactions",
                column: "FosterConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Consignments_FosterConsignmentID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_FosterConsignmentID",
                table: "Transactions");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class AddOnConstrains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOn_ConsignmentKois_ConsignmentKoiID",
                table: "AddOn");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsignmentKois_Consignments_ConsignmentID",
                table: "ConsignmentKois");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "ConsignmentID1",
                table: "ConsignmentKois",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentKois_ConsignmentID1",
                table: "ConsignmentKois",
                column: "ConsignmentID1");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOn_ConsignmentKois_ConsignmentKoiID",
                table: "AddOn",
                column: "ConsignmentKoiID",
                principalTable: "ConsignmentKois",
                principalColumn: "ConsignmentKoiID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsignmentKois_Consignments_ConsignmentID",
                table: "ConsignmentKois",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsignmentKois_Consignments_ConsignmentID1",
                table: "ConsignmentKois",
                column: "ConsignmentID1",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOn_ConsignmentKois_ConsignmentKoiID",
                table: "AddOn");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsignmentKois_Consignments_ConsignmentID",
                table: "ConsignmentKois");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsignmentKois_Consignments_ConsignmentID1",
                table: "ConsignmentKois");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_ConsignmentKois_ConsignmentID1",
                table: "ConsignmentKois");

            migrationBuilder.DropColumn(
                name: "ConsignmentID1",
                table: "ConsignmentKois");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOn_ConsignmentKois_ConsignmentKoiID",
                table: "AddOn",
                column: "ConsignmentKoiID",
                principalTable: "ConsignmentKois",
                principalColumn: "ConsignmentKoiID");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsignmentKois_Consignments_ConsignmentID",
                table: "ConsignmentKois",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

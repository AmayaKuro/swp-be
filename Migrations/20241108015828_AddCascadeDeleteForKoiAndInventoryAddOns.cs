using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteForKoiAndInventoryAddOns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOn_Kois_KoiID",
                table: "AddOn");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiInventory_AddOn_AddOnID",
                table: "KoiInventory");

            migrationBuilder.DropIndex(
                name: "IX_KoiInventory_AddOnID",
                table: "KoiInventory");

            migrationBuilder.DropColumn(
                name: "AddOnID",
                table: "KoiInventory");

            migrationBuilder.AddColumn<int>(
                name: "KoiInventoryID",
                table: "AddOn",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddOn_KoiInventoryID",
                table: "AddOn",
                column: "KoiInventoryID",
                unique: true,
                filter: "[KoiInventoryID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOn_KoiInventory_KoiInventoryID",
                table: "AddOn",
                column: "KoiInventoryID",
                principalTable: "KoiInventory",
                principalColumn: "KoiInventoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddOn_Kois_KoiID",
                table: "AddOn",
                column: "KoiID",
                principalTable: "Kois",
                principalColumn: "KoiID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOn_KoiInventory_KoiInventoryID",
                table: "AddOn");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOn_Kois_KoiID",
                table: "AddOn");

            migrationBuilder.DropIndex(
                name: "IX_AddOn_KoiInventoryID",
                table: "AddOn");

            migrationBuilder.DropColumn(
                name: "KoiInventoryID",
                table: "AddOn");

            migrationBuilder.AddColumn<int>(
                name: "AddOnID",
                table: "KoiInventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KoiInventory_AddOnID",
                table: "KoiInventory",
                column: "AddOnID");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOn_Kois_KoiID",
                table: "AddOn",
                column: "KoiID",
                principalTable: "Kois",
                principalColumn: "KoiID");

            migrationBuilder.AddForeignKey(
                name: "FK_KoiInventory_AddOn_AddOnID",
                table: "KoiInventory",
                column: "AddOnID",
                principalTable: "AddOn",
                principalColumn: "AddOnID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

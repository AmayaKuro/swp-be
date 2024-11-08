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
                name: "FK_AddOn_Kois_KoiID",
                table: "AddOn");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiInventory_AddOn_AddOnID",
                table: "KoiInventory");

            migrationBuilder.DropIndex(
                name: "IX_KoiInventory_AddOnID",
                table: "KoiInventory");

            migrationBuilder.DropIndex(
                name: "IX_AddOn_ConsignmentKoiID",
                table: "AddOn");

            migrationBuilder.DropIndex(
                name: "IX_AddOn_KoiID",
                table: "AddOn");

            migrationBuilder.RenameColumn(
                name: "AddOnID",
                table: "KoiInventory",
                newName: "AddOnId");

            migrationBuilder.AddColumn<int>(
                name: "AddOnId",
                table: "Kois",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddOnId",
                table: "KoiInventory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AddOnId",
                table: "ConsignmentKois",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KoiInventoryID",
                table: "AddOn",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kois_AddOnId",
                table: "Kois",
                column: "AddOnId",
                unique: true,
                filter: "[AddOnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KoiInventory_AddOnId",
                table: "KoiInventory",
                column: "AddOnId",
                unique: true,
                filter: "[AddOnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentKois_AddOnId",
                table: "ConsignmentKois",
                column: "AddOnId",
                unique: true,
                filter: "[AddOnId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsignmentKois_AddOn_AddOnId",
                table: "ConsignmentKois",
                column: "AddOnId",
                principalTable: "AddOn",
                principalColumn: "AddOnID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiInventory_AddOn_AddOnId",
                table: "KoiInventory",
                column: "AddOnId",
                principalTable: "AddOn",
                principalColumn: "AddOnID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kois_AddOn_AddOnId",
                table: "Kois",
                column: "AddOnId",
                principalTable: "AddOn",
                principalColumn: "AddOnID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsignmentKois_AddOn_AddOnId",
                table: "ConsignmentKois");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiInventory_AddOn_AddOnId",
                table: "KoiInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Kois_AddOn_AddOnId",
                table: "Kois");

            migrationBuilder.DropIndex(
                name: "IX_Kois_AddOnId",
                table: "Kois");

            migrationBuilder.DropIndex(
                name: "IX_KoiInventory_AddOnId",
                table: "KoiInventory");

            migrationBuilder.DropIndex(
                name: "IX_ConsignmentKois_AddOnId",
                table: "ConsignmentKois");

            migrationBuilder.DropColumn(
                name: "AddOnId",
                table: "Kois");

            migrationBuilder.DropColumn(
                name: "AddOnId",
                table: "ConsignmentKois");

            migrationBuilder.DropColumn(
                name: "KoiInventoryID",
                table: "AddOn");

            migrationBuilder.RenameColumn(
                name: "AddOnId",
                table: "KoiInventory",
                newName: "AddOnID");

            migrationBuilder.AlterColumn<int>(
                name: "AddOnID",
                table: "KoiInventory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KoiInventory_AddOnID",
                table: "KoiInventory",
                column: "AddOnID");

            migrationBuilder.CreateIndex(
                name: "IX_AddOn_ConsignmentKoiID",
                table: "AddOn",
                column: "ConsignmentKoiID",
                unique: true,
                filter: "[ConsignmentKoiID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AddOn_KoiID",
                table: "AddOn",
                column: "KoiID",
                unique: true,
                filter: "[KoiID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOn_ConsignmentKois_ConsignmentKoiID",
                table: "AddOn",
                column: "ConsignmentKoiID",
                principalTable: "ConsignmentKois",
                principalColumn: "ConsignmentKoiID");

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

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsForClarityAndTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddOn",
                table: "Kois");

            migrationBuilder.DropColumn(
                name: "AddOn",
                table: "ConsignmentKois");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Batches",
                newName: "QuantityPerBatch");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Batches",
                newName: "PricePerBatch");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Deliveries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AddOn",
                columns: table => new
                {
                    AddOnID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnershipCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KoiID = table.Column<int>(type: "int", nullable: true),
                    ConsignmentKoiID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOn", x => x.AddOnID);
                    table.ForeignKey(
                        name: "FK_AddOn_ConsignmentKois_ConsignmentKoiID",
                        column: x => x.ConsignmentKoiID,
                        principalTable: "ConsignmentKois",
                        principalColumn: "ConsignmentKoiID");
                    table.ForeignKey(
                        name: "FK_AddOn_Kois_KoiID",
                        column: x => x.KoiID,
                        principalTable: "Kois",
                        principalColumn: "KoiID");
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddOn");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Deliveries");

            migrationBuilder.RenameColumn(
                name: "QuantityPerBatch",
                table: "Batches",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "PricePerBatch",
                table: "Batches",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "AddOn",
                table: "Kois",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Deliveries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AddOn",
                table: "ConsignmentKois",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

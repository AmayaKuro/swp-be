using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class AddKoiInventoryAndConsignmentPriceListAndUpdateSomeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StaffZone",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonImage",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonImage",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConsignmentPriceListID",
                table: "Consignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ConsignmentPriceList",
                columns: table => new
                {
                    ConsignmentPriceListID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsignmentPriceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerDay = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsignmentPriceList", x => x.ConsignmentPriceListID);
                });

            migrationBuilder.CreateTable(
                name: "KoiInventory",
                columns: table => new
                {
                    KoiInventoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DailyFeedAmount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Personality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SelectionRate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Species = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddOnID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiInventory", x => x.KoiInventoryID);
                    table.ForeignKey(
                        name: "FK_KoiInventory_AddOn_AddOnID",
                        column: x => x.AddOnID,
                        principalTable: "AddOn",
                        principalColumn: "AddOnID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KoiInventory_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consignments_ConsignmentPriceListID",
                table: "Consignments",
                column: "ConsignmentPriceListID");

            migrationBuilder.CreateIndex(
                name: "IX_KoiInventory_AddOnID",
                table: "KoiInventory",
                column: "AddOnID");

            migrationBuilder.CreateIndex(
                name: "IX_KoiInventory_CustomerID",
                table: "KoiInventory",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Consignments_ConsignmentPriceList_ConsignmentPriceListID",
                table: "Consignments",
                column: "ConsignmentPriceListID",
                principalTable: "ConsignmentPriceList",
                principalColumn: "ConsignmentPriceListID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consignments_ConsignmentPriceList_ConsignmentPriceListID",
                table: "Consignments");

            migrationBuilder.DropTable(
                name: "ConsignmentPriceList");

            migrationBuilder.DropTable(
                name: "KoiInventory");

            migrationBuilder.DropIndex(
                name: "IX_Consignments_ConsignmentPriceListID",
                table: "Consignments");

            migrationBuilder.DropColumn(
                name: "StaffZone",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReasonImage",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ReasonImage",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ConsignmentPriceListID",
                table: "Consignments");
        }
    }
}

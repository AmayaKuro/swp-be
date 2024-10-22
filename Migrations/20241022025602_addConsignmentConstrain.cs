using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class addConsignmentConstrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Kois");

            migrationBuilder.RenameColumn(
                name: "PricePerDay",
                table: "FosterKois",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Consignments",
                newName: "FosterPrice");

            migrationBuilder.AddColumn<string>(
                name: "AddOn",
                table: "FosterKois",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Consignments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddOn",
                table: "FosterKois");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Consignments");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "FosterKois",
                newName: "PricePerDay");

            migrationBuilder.RenameColumn(
                name: "FosterPrice",
                table: "Consignments",
                newName: "TotalPrice");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Kois",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

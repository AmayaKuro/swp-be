using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class bigUpdateForOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FosterBatches_Consignments_ConsignmentID",
                table: "FosterBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_FosterKois_Consignments_ConsignmentID",
                table: "FosterKois");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FosterKois",
                table: "FosterKois");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FosterBatches",
                table: "FosterBatches");

            migrationBuilder.RenameTable(
                name: "FosterKois",
                newName: "ConsignmentKois");

            migrationBuilder.RenameTable(
                name: "FosterBatches",
                newName: "FosterBatch");

            migrationBuilder.RenameColumn(
                name: "FosterKoiID",
                table: "ConsignmentKois",
                newName: "ConsignmentKoiID");

            migrationBuilder.RenameIndex(
                name: "IX_FosterKois_ConsignmentID",
                table: "ConsignmentKois",
                newName: "IX_ConsignmentKois_ConsignmentID");

            migrationBuilder.RenameIndex(
                name: "IX_FosterBatches_ConsignmentID",
                table: "FosterBatch",
                newName: "IX_FosterBatch_ConsignmentID");

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "Transactions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "TotalAmount",
                table: "Orders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "Price",
                table: "OrderDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "ConsignmentKoiID",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Price",
                table: "Kois",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Kois",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FosterPrice",
                table: "Consignments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "Price",
                table: "Batches",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "Price",
                table: "ConsignmentKois",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ConsignmentKois",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PricePerDay",
                table: "FosterBatch",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsignmentKois",
                table: "ConsignmentKois",
                column: "ConsignmentKoiID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FosterBatch",
                table: "FosterBatch",
                column: "FosterBatchID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ConsignmentKoiID",
                table: "OrderDetails",
                column: "ConsignmentKoiID");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsignmentKois_Consignments_ConsignmentID",
                table: "ConsignmentKois",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FosterBatch_Consignments_ConsignmentID",
                table: "FosterBatch",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ConsignmentKois_ConsignmentKoiID",
                table: "OrderDetails",
                column: "ConsignmentKoiID",
                principalTable: "ConsignmentKois",
                principalColumn: "ConsignmentKoiID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsignmentKois_Consignments_ConsignmentID",
                table: "ConsignmentKois");

            migrationBuilder.DropForeignKey(
                name: "FK_FosterBatch_Consignments_ConsignmentID",
                table: "FosterBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ConsignmentKois_ConsignmentKoiID",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ConsignmentKoiID",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FosterBatch",
                table: "FosterBatch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsignmentKois",
                table: "ConsignmentKois");

            migrationBuilder.DropColumn(
                name: "ConsignmentKoiID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Kois");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ConsignmentKois");

            migrationBuilder.RenameTable(
                name: "FosterBatch",
                newName: "FosterBatches");

            migrationBuilder.RenameTable(
                name: "ConsignmentKois",
                newName: "FosterKois");

            migrationBuilder.RenameIndex(
                name: "IX_FosterBatch_ConsignmentID",
                table: "FosterBatches",
                newName: "IX_FosterBatches_ConsignmentID");

            migrationBuilder.RenameColumn(
                name: "ConsignmentKoiID",
                table: "FosterKois",
                newName: "FosterKoiID");

            migrationBuilder.RenameIndex(
                name: "IX_ConsignmentKois_ConsignmentID",
                table: "FosterKois",
                newName: "IX_FosterKois_ConsignmentID");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Kois",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "FosterPrice",
                table: "Consignments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Batches",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerDay",
                table: "FosterBatches",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "FosterKois",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FosterBatches",
                table: "FosterBatches",
                column: "FosterBatchID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FosterKois",
                table: "FosterKois",
                column: "FosterKoiID");

            migrationBuilder.AddForeignKey(
                name: "FK_FosterBatches_Consignments_ConsignmentID",
                table: "FosterBatches",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FosterKois_Consignments_ConsignmentID",
                table: "FosterKois",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

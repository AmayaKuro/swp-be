using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class updateTransactionAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Consignments_FosterConsignmentID",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Transactions",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "FosterConsignmentID",
                table: "Transactions",
                newName: "ConsignmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_FosterConsignmentID",
                table: "Transactions",
                newName: "IX_Transactions_ConsignmentID");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Orders",
                newName: "CreateAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndAt",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Kois",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Kois",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Consignments_ConsignmentID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "EndAt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Kois");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Transactions",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "ConsignmentID",
                table: "Transactions",
                newName: "FosterConsignmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ConsignmentID",
                table: "Transactions",
                newName: "IX_Transactions_FosterConsignmentID");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Orders",
                newName: "Date");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Kois",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Consignments_FosterConsignmentID",
                table: "Transactions",
                column: "FosterConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID");
        }
    }
}

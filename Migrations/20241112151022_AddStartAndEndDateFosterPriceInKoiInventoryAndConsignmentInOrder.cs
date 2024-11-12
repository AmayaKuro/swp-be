using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class AddStartAndEndDateFosterPriceInKoiInventoryAndConsignmentInOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consignments_Orders_OrderID",
                table: "Consignments");

            migrationBuilder.DropIndex(
                name: "IX_Consignments_OrderID",
                table: "Consignments");

            migrationBuilder.AddColumn<int>(
                name: "ConsignmentID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "KoiInventory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FosterPrice",
                table: "KoiInventory",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "KoiInventory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3001,
                column: "Password",
                value: "$2a$11$waNOToFRcRSU27NRqBoTmuTts2Uga083BRaxELC4oRhu7wTLUnirO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3002,
                column: "Password",
                value: "$2a$11$c4QmZ1giDGd40W7ZnXV7P.mjd3rUD9gi52SV2logEhUt3qVMS.qzy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3003,
                column: "Password",
                value: "$2a$11$TF3R5EnCeU8mqV9B.YNLzO3xVSp1sx2vZmmgB/8ZA.WHEmivcbQUi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3004,
                column: "Password",
                value: "$2a$11$7wx82eLB4pmiXqYwTCTtcOdpc9FBEhgDL26qq.EltGFkMouIr.wfq");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ConsignmentID",
                table: "Orders",
                column: "ConsignmentID",
                unique: true,
                filter: "[ConsignmentID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Consignments_ConsignmentID",
                table: "Orders",
                column: "ConsignmentID",
                principalTable: "Consignments",
                principalColumn: "ConsignmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Consignments_ConsignmentID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ConsignmentID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ConsignmentID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "KoiInventory");

            migrationBuilder.DropColumn(
                name: "FosterPrice",
                table: "KoiInventory");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "KoiInventory");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3001,
                column: "Password",
                value: "$2a$11$npRibh4p9.IM/LORnkyjN.shsiaBvs1CXf0zEdKwrh3apO.iL63y2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3002,
                column: "Password",
                value: "$2a$11$IRm9Msgo4d2zr6Hx011wZe.FHiyl8m38gcn1p2EEw1woxtOnLEv..");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3003,
                column: "Password",
                value: "$2a$11$1hzf/L48uj9OQy7ejaqSyuEUv8qcQAhHRLQeXuxxlzuZbdBnQMirS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3004,
                column: "Password",
                value: "$2a$11$CrqHZpiIsIID5Yq0AjQHx.ZyquwctpPCUyjZewF32kWbgoZfrLVTi");

            migrationBuilder.CreateIndex(
                name: "IX_Consignments_OrderID",
                table: "Consignments",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Consignments_Orders_OrderID",
                table: "Consignments",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }
    }
}

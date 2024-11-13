using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerIDToPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "Promotions",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3001,
                column: "Password",
                value: "$2a$11$JuCmfGHoi0Zh8JVCxsJD1.BJ4tPzdvirHbN4Dz4RKgbOYpvFLr7G2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3002,
                column: "Password",
                value: "$2a$11$F5NgIK89Ns3xBj0eNi5la.NtVZFBWjnVXFZVKWo7j0RoFc.SjbA2S");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3003,
                column: "Password",
                value: "$2a$11$wtSNvJkk/6zVupcQK7soh.s2ShL.RhpBd/IpHqBdPW/23PlBZhz1K");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3004,
                column: "Password",
                value: "$2a$11$XpgCF9VvdyCmzw34AdhrZulCNvRSzxhAPxVSVEJj20OhrHgiS8dJG");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_CustomerID",
                table: "Promotions",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Customers_CustomerID",
                table: "Promotions",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Customers_CustomerID",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_CustomerID",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Promotions");

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
        }
    }
}

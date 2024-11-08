using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace swp_be.Migrations
{
    /// <inheritdoc />
    public partial class reInitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConsignmentPriceList",
                columns: new[] { "ConsignmentPriceListID", "ConsignmentPriceName", "PricePerDay" },
                values: new object[] { 1, "gay tra nhieu tien", 100000L });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "PaymentMethodID", "MethodName" },
                values: new object[,]
                {
                    { 1, "Cash" },
                    { 2, "VNPay" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Address", "Email", "Name", "Password", "Phone", "Role", "Username" },
                values: new object[,]
                {
                    { 3001, null, null, "Admin", "$2a$11$UsBlcZvVRSrDNdq.3.Dm2Osl.Wa.wP/.WLYtKzNUpz5KoipzD71lq", null, 0, "admin" },
                    { 3002, null, null, "Staff", "$2a$11$5lAjnn.etoeKOW3yhEX/JuS8hb3Zj/9J.9uqE698rg7ZK0G7jkCI6", null, 1, "staff" },
                    { 3003, null, null, "Customer", "$2a$11$XAtmAWoPTl1Iq5uV1mtrnO.oefA8.Mo3Aa1TBkbLp8xaCfQdHPOAG", null, 2, "customer" },
                    { 3004, null, null, "String", "$2a$11$Xcx.h5m.vjNLX.Wg5N6Fj.TVn9diErlgfF2NiC7mxrKqAIC6Xcc4y", null, 0, "string" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "UserID", "LoyaltyPoints" },
                values: new object[,]
                {
                    { 3003, 0 },
                    { 3004, 0 }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "UserID", "StaffZone" },
                values: new object[,]
                {
                    { 3001, null },
                    { 3002, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConsignmentPriceList",
                keyColumn: "ConsignmentPriceListID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "UserID",
                keyValue: 3003);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "UserID",
                keyValue: 3004);

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "UserID",
                keyValue: 3001);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "UserID",
                keyValue: 3002);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3001);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3002);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3003);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3004);
        }
    }
}

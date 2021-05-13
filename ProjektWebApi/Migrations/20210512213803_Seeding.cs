using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjektWebApi.Migrations
{
    public partial class Seeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0838308e-20b1-49dc-984d-e79e64df3a1d", 0, "15363692-7181-4372-9eaa-8c33aa943332", null, false, "Test", "Testsson", false, null, null, null, "AE5KKzLL1+U4YMEKVVvX/t8oSepjsfxAhwX/GpYvK1PckogPmCMhc+t1CMXZNpTzYg==", null, false, "0198fd72-b697-4ca2-9d82-bf680b3920ec", false, "testuser" });

            migrationBuilder.InsertData(
                table: "Message",
                columns: new[] { "Id", "Author", "Body", "Title" },
                values: new object[] { 2, "Unknown Author", "Bästa stället att dricka öl!", "Andra långgatan" });

            migrationBuilder.InsertData(
                table: "Message",
                columns: new[] { "Id", "Author", "Body", "Title" },
                values: new object[] { 1, "Unknown Author", "Här bor Bella! Stay away", "Bellas place" });

            migrationBuilder.InsertData(
                table: "GeoMessages",
                columns: new[] { "Id", "Latitude", "Longitude", "MessageId" },
                values: new object[] { 1, 11.970617969653047, 57.873718295961204, 1 });

            migrationBuilder.InsertData(
                table: "GeoMessages",
                columns: new[] { "Id", "Latitude", "Longitude", "MessageId" },
                values: new object[] { 2, 11.946499084988522, 57.699100041459346, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0838308e-20b1-49dc-984d-e79e64df3a1d");

            migrationBuilder.DeleteData(
                table: "GeoMessages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GeoMessages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Message",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Message",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

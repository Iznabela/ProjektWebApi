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
                values: new object[] { "c5b0bb0c-e5a0-4f88-b77e-9f84ba11efa5", 0, "a780f3db-fc43-4ade-bbf4-ebc790290154", null, false, "Test", "Testsson", false, null, null, null, "AE3KdPLUMUqzflwOVV0MFGm0C0gZmpvviCKc2CLqIV/woqzvV4nkX7JjkBg/cplg8A==", null, false, "c1876dab-9def-4d9b-87b1-9cbfbda83903", false, "testuser" });

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
                keyValue: "c5b0bb0c-e5a0-4f88-b77e-9f84ba11efa5");

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

using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjektWebApi.Migrations
{
    public partial class DefaultMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Message",
                columns: new[] { "Id", "Author", "Body", "Title" },
                values: new object[] { 1, "Unknown Author", "Här bor Bella! Stay away", "Bellas place" });

            migrationBuilder.InsertData(
                table: "Message",
                columns: new[] { "Id", "Author", "Body", "Title" },
                values: new object[] { 2, "Unknown Author", "Bästa stället att dricka öl!", "Andra långgatan" });

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

using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjektWebApi.Migrations
{
    public partial class newModelForVersioning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoMessagesV2");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "GeoMessages");

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "GeoMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeoMessages_MessageId",
                table: "GeoMessages",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoMessages_Message_MessageId",
                table: "GeoMessages",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoMessages_Message_MessageId",
                table: "GeoMessages");

            migrationBuilder.DropIndex(
                name: "IX_GeoMessages_MessageId",
                table: "GeoMessages");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "GeoMessages");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "GeoMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GeoMessagesV2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoMessagesV2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoMessagesV2_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeoMessagesV2_MessageId",
                table: "GeoMessagesV2",
                column: "MessageId");
        }
    }
}

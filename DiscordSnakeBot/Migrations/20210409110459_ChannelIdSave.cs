using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordSnakeBot.Migrations
{
    public partial class ChannelIdSave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "GameChannelId",
                table: "Servers",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameChannelId",
                table: "Servers");
        }
    }
}

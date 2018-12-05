using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CampaignKit.WorldMap.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AdjustedSize = table.Column<int>(nullable: false),
                    ContentType = table.Column<string>(nullable: true),
                    Copyright = table.Column<string>(nullable: true),
                    CreationTimestamp = table.Column<DateTime>(nullable: false),
                    FileExtension = table.Column<string>(nullable: true),
                    MaxZoomLevel = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RepeatMapInX = table.Column<bool>(nullable: false),
                    Secret = table.Column<string>(nullable: true),
                    ThumbnailPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Maps");
        }
    }
}

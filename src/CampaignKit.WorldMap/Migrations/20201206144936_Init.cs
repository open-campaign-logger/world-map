using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace CampaignKit.WorldMap.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    MapId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdjustedSize = table.Column<int>(nullable: false),
                    ContentType = table.Column<string>(nullable: true),
                    Copyright = table.Column<string>(nullable: true),
                    CreationTimestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    FileExtension = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    MarkerData = table.Column<string>(nullable: true),
                    MaxZoomLevel = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RepeatMapInX = table.Column<bool>(nullable: false),
                    ShareKey = table.Column<string>(nullable: true),
                    ThumbnailPath = table.Column<string>(nullable: true),
                    UpdateTimestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    WorldFolderPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.MapId);
                });

            migrationBuilder.CreateTable(
                name: "Tiles",
                columns: table => new
                {
                    TileId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompletionTimestamp = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CreationTimestamp = table.Column<DateTime>(type: "DateTime", nullable: false),
                    MapId = table.Column<int>(nullable: false),
                    TileSize = table.Column<int>(nullable: false),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    ZoomLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiles", x => x.TileId);
                    table.ForeignKey(
                        name: "FK_Tiles_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tiles_MapId",
                table: "Tiles",
                column: "MapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tiles");

            migrationBuilder.DropTable(
                name: "Maps");
        }
    }
}

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
                    MapId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdjustedSize = table.Column<int>(nullable: false),
                    ContentType = table.Column<string>(nullable: true),
                    Copyright = table.Column<string>(nullable: true),
                    CreationTimestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    FileExtension = table.Column<string>(nullable: true),
                    MaxZoomLevel = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RepeatMapInX = table.Column<bool>(nullable: false),
                    Secret = table.Column<string>(nullable: true),
                    ThumbnailPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.MapId);
                });

            migrationBuilder.CreateTable(
                name: "Markers",
                columns: table => new
                {
                    MarkerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    x = table.Column<int>(nullable: false),
                    y = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    MarkerData = table.Column<string>(nullable: true),
                    MapId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markers", x => x.MarkerId);
                    table.ForeignKey(
                        name: "FK_Markers_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tiles",
                columns: table => new
                {
                    TileId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MapId = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    CreationTimestamp = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CompletionTimestamp = table.Column<DateTime>(type: "DateTime", nullable: false),
                    TileSize = table.Column<int>(nullable: false)
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

            migrationBuilder.InsertData(
                table: "Maps",
                columns: new[] { "MapId", "AdjustedSize", "ContentType", "Copyright", "CreationTimestamp", "FileExtension", "MaxZoomLevel", "Name", "RepeatMapInX", "Secret", "ThumbnailPath" },
                values: new object[] { 1, 4000, "image/jpeg", null, new DateTime(2018, 12, 8, 1, 3, 35, 474, DateTimeKind.Utc).AddTicks(8587), ".jpg", 4, "Sample", false, "lNtqjEVQ", "~/world/1/0/zoom-level.png" });

            migrationBuilder.CreateIndex(
                name: "IX_Markers_MapId",
                table: "Markers",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Tiles_MapId",
                table: "Tiles",
                column: "MapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Markers");

            migrationBuilder.DropTable(
                name: "Tiles");

            migrationBuilder.DropTable(
                name: "Maps");
        }
    }
}

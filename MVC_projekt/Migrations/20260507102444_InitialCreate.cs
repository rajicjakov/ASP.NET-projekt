using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_projekt.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tabs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Artist = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StringTuning = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BPM = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tabs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tabs_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabMeasures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TabId = table.Column<int>(type: "int", nullable: false),
                    MeasureNumber = table.Column<int>(type: "int", nullable: false),
                    TimeSignatureTop = table.Column<int>(type: "int", nullable: false),
                    TimeSignatureBottom = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabMeasures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TabMeasures_Tabs_TabId",
                        column: x => x.TabId,
                        principalTable: "Tabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabColumns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TabMeasureId = table.Column<int>(type: "int", nullable: false),
                    ColumnNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TabColumns_TabMeasures_TabMeasureId",
                        column: x => x.TabMeasureId,
                        principalTable: "TabMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Durations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Base = table.Column<int>(type: "int", nullable: false),
                    IsDotted = table.Column<bool>(type: "bit", nullable: false),
                    TabColumnId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Durations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Durations_TabColumns_TabColumnId",
                        column: x => x.TabColumnId,
                        principalTable: "TabColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TabColumnId = table.Column<int>(type: "int", nullable: false),
                    StringNumber = table.Column<int>(type: "int", nullable: false),
                    Fret = table.Column<int>(type: "int", nullable: false),
                    PalmMuted = table.Column<bool>(type: "bit", nullable: false),
                    HammerOn = table.Column<bool>(type: "bit", nullable: false),
                    PullOff = table.Column<bool>(type: "bit", nullable: false),
                    Bend = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TabNotes_TabColumns_TabColumnId",
                        column: x => x.TabColumnId,
                        principalTable: "TabColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Durations_TabColumnId",
                table: "Durations",
                column: "TabColumnId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TabColumns_TabMeasureId",
                table: "TabColumns",
                column: "TabMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_TabMeasures_TabId",
                table: "TabMeasures",
                column: "TabId");

            migrationBuilder.CreateIndex(
                name: "IX_TabNotes_TabColumnId",
                table: "TabNotes",
                column: "TabColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Tabs_CreatorId",
                table: "Tabs",
                column: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Durations");

            migrationBuilder.DropTable(
                name: "TabNotes");

            migrationBuilder.DropTable(
                name: "TabColumns");

            migrationBuilder.DropTable(
                name: "TabMeasures");

            migrationBuilder.DropTable(
                name: "Tabs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

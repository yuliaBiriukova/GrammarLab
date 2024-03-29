using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammarLab.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCompletedTestToTestResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedTestExercises");

            migrationBuilder.DropTable(
                name: "CompletedTests");

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Percentage = table.Column<int>(type: "int", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResults_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResults_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResultExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Task = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserAnswer = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TestResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResultExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResultExercises_TestResults_TestResultId",
                        column: x => x.TestResultId,
                        principalTable: "TestResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestResultExercises_TestResultId",
                table: "TestResultExercises",
                column: "TestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TopicId",
                table: "TestResults",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_UserId",
                table: "TestResults",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestResultExercises");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.CreateTable(
                name: "CompletedTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Percentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedTests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedTests_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedTestExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompletedTestId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Task = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserAnswer = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedTestExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedTestExercises_CompletedTests_CompletedTestId",
                        column: x => x.CompletedTestId,
                        principalTable: "CompletedTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedTestExercises_CompletedTestId",
                table: "CompletedTestExercises",
                column: "CompletedTestId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedTests_TopicId",
                table: "CompletedTests",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedTests_UserId",
                table: "CompletedTests",
                column: "UserId");
        }
    }
}

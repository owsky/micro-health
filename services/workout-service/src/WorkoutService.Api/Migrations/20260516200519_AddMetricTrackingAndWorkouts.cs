using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutService.Migrations
{
    /// <inheritdoc />
    public partial class AddMetricTrackingAndWorkouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingType",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSets",
                columns: table => new
                {
                    WorkoutId = table.Column<long>(type: "bigint", nullable: false),
                    ExerciseId = table.Column<long>(type: "bigint", nullable: false),
                    SetNumber = table.Column<int>(type: "int", nullable: false),
                    IsWarmup = table.Column<bool>(type: "bit", nullable: false),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SetType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DistanceAndTimeSet_DurationSeconds = table.Column<int>(type: "int", nullable: true),
                    RepsSet_Reps = table.Column<int>(type: "int", nullable: true),
                    TimeSet_DurationSeconds = table.Column<int>(type: "int", nullable: true),
                    Reps = table.Column<int>(type: "int", nullable: true),
                    DurationSeconds = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSets", x => new { x.WorkoutId, x.ExerciseId, x.SetNumber });
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_ExerciseId",
                table: "WorkoutSets",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutSets");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropColumn(
                name: "TrackingType",
                table: "Exercises");
        }
    }
}

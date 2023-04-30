using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflowAPI.Migrations
{
    /// <inheritdoc />
    public partial class RatingSumAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_QuestionId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "RatingSum",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_QuestionId",
                table: "Ratings",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_QuestionId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RatingSum",
                table: "Questions");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_QuestionId",
                table: "Ratings",
                column: "QuestionId",
                unique: true,
                filter: "[QuestionId] IS NOT NULL");
        }
    }
}

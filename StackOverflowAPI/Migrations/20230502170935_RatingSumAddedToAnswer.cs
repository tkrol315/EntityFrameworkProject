using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflowAPI.Migrations
{
    /// <inheritdoc />
    public partial class RatingSumAddedToAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Answers_AnswerId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Questions_QuestionId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_AnswerId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "RatingSum",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AnswerId",
                table: "Ratings",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Answers_AnswerId",
                table: "Ratings",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Questions_QuestionId",
                table: "Ratings",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Answers_AnswerId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Questions_QuestionId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_AnswerId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RatingSum",
                table: "Answers");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AnswerId",
                table: "Ratings",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Answers_AnswerId",
                table: "Ratings",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Questions_QuestionId",
                table: "Ratings",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

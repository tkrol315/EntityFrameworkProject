using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflowAPI.Migrations
{
    /// <inheritdoc />
    public partial class answerAndQuestionOnDeleteSetNull : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_Ratings_QuestionId",
                table: "Ratings");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnswerId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AnswerId",
                table: "Ratings",
                column: "AnswerId",
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_QuestionId",
                table: "Ratings",
                column: "QuestionId",
                unique: true,
                filter: "[QuestionId] IS NOT NULL");

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

            migrationBuilder.DropIndex(
                name: "IX_Ratings_QuestionId",
                table: "Ratings");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AnswerId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AnswerId",
                table: "Ratings",
                column: "AnswerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_QuestionId",
                table: "Ratings",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Answers_AnswerId",
                table: "Ratings",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Questions_QuestionId",
                table: "Ratings",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

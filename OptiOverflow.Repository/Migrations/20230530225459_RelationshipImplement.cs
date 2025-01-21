using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugBusters.Server.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipImplement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteType",
                table: "Vote");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Answer",
                newName: "UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsUpVote",
                table: "Vote",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Vote",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Vote_QuestionId",
                table: "Vote",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Question_QuestionId",
                table: "Answer",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Question_QuestionId",
                table: "Vote",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Question_QuestionId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Question_QuestionId",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Vote_QuestionId",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "IsUpVote",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Vote");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Answer",
                newName: "CreatedBy");

            migrationBuilder.AddColumn<int>(
                name: "VoteType",
                table: "Vote",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

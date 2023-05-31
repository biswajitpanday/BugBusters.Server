using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiOverflow.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationToVoteAndAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Vote_AnswerId",
                table: "Vote",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Answer_AnswerId",
                table: "Vote",
                column: "AnswerId",
                principalTable: "Answer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Answer_AnswerId",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Vote_AnswerId",
                table: "Vote");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugBusters.Server.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationPropertyToAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Answer");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Answer",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_CreatedById",
                table: "Answer",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_AspNetUsers_CreatedById",
                table: "Answer",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_AspNetUsers_CreatedById",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_CreatedById",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Answer");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Answer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

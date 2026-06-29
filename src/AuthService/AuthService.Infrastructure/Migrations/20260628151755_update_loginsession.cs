using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_loginsession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginSession_AspNetUsers_UserId",
                table: "LoginSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginSession",
                table: "LoginSession");

            migrationBuilder.RenameTable(
                name: "LoginSession",
                newName: "LoginSessions");

            migrationBuilder.RenameIndex(
                name: "IX_LoginSession_UserId",
                table: "LoginSessions",
                newName: "IX_LoginSessions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LoginSession_SessionId",
                table: "LoginSessions",
                newName: "IX_LoginSessions_SessionId");

            migrationBuilder.AlterColumn<string>(
                name: "Otp",
                table: "LoginSessions",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginSessions",
                table: "LoginSessions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginSessions_AspNetUsers_UserId",
                table: "LoginSessions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginSessions_AspNetUsers_UserId",
                table: "LoginSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginSessions",
                table: "LoginSessions");

            migrationBuilder.RenameTable(
                name: "LoginSessions",
                newName: "LoginSession");

            migrationBuilder.RenameIndex(
                name: "IX_LoginSessions_UserId",
                table: "LoginSession",
                newName: "IX_LoginSession_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LoginSessions_SessionId",
                table: "LoginSession",
                newName: "IX_LoginSession_SessionId");

            migrationBuilder.AlterColumn<string>(
                name: "Otp",
                table: "LoginSession",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginSession",
                table: "LoginSession",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginSession_AspNetUsers_UserId",
                table: "LoginSession",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

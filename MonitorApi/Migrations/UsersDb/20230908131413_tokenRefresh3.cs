using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitorApi.Migrations.UsersDb
{
    public partial class tokenRefresh3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshTokens_AspNetUsers_UserId",
                table: "refreshTokens");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "refreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_refreshTokens_AspNetUsers_UserId",
                table: "refreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshTokens_AspNetUsers_UserId",
                table: "refreshTokens");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "refreshTokens",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_refreshTokens_AspNetUsers_UserId",
                table: "refreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

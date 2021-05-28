using Microsoft.EntityFrameworkCore.Migrations;

namespace MailingApi.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "usr_name",
                table: "MailUser",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailUser_usr_name",
                table: "MailUser",
                column: "usr_name",
                unique: true,
                filter: "[usr_name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MailUser_usr_name",
                table: "MailUser");

            migrationBuilder.AlterColumn<string>(
                name: "usr_name",
                table: "MailUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}

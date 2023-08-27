using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkinsSite.Migrations
{
    public partial class ImagemNome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemThumbnailUrl",
                table: "Skins");

            migrationBuilder.DropColumn(
                name: "ImagemUrl",
                table: "Skins");

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoCurta",
                table: "Skins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "ImagemNome",
                table: "Skins",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemNome",
                table: "Skins");

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoCurta",
                table: "Skins",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagemThumbnailUrl",
                table: "Skins",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagemUrl",
                table: "Skins",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}

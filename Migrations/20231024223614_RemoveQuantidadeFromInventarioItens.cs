using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkinsSite.Migrations
{
    public partial class RemoveQuantidadeFromInventarioItens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "InventarioItens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "InventarioItens",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

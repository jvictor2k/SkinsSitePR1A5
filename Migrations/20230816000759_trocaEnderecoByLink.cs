using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkinsSite.Migrations
{
    public partial class trocaEnderecoByLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Endereco1",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Endereco2",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pedidos");

            migrationBuilder.AddColumn<string>(
                name: "TradeLink",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TradeLink",
                table: "Pedidos");

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Pedidos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Pedidos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco1",
                table: "Pedidos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco2",
                table: "Pedidos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Pedidos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}

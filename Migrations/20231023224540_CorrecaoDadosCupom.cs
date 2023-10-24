using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkinsSite.Migrations
{
    public partial class CorrecaoDadosCupom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CuponsAplicados",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DescontoTotal",
                table: "Pedidos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                table: "PedidoDetalhes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                table: "InventarioItens",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CupomUsado",
                table: "CarrinhoCompraItens",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuponsAplicados",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "DescontoTotal",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Desconto",
                table: "PedidoDetalhes");

            migrationBuilder.DropColumn(
                name: "Desconto",
                table: "InventarioItens");

            migrationBuilder.DropColumn(
                name: "CupomUsado",
                table: "CarrinhoCompraItens");
        }
    }
}

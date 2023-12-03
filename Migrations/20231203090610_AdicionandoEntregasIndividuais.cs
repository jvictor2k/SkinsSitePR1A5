using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkinsSite.Migrations
{
    public partial class AdicionandoEntregasIndividuais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PedidoDetalheEntregueEm",
                table: "PedidoDetalhes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ItemEntregueEm",
                table: "InventarioItens",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PedidoDetalheEntregueEm",
                table: "PedidoDetalhes");

            migrationBuilder.DropColumn(
                name: "ItemEntregueEm",
                table: "InventarioItens");
        }
    }
}

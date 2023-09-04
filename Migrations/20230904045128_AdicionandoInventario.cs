using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkinsSite.Migrations
{
    public partial class AdicionandoInventario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventarios",
                columns: table => new
                {
                    InventarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalItensInventario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventarios", x => x.InventarioId);
                });

            migrationBuilder.CreateTable(
                name: "InventarioItens",
                columns: table => new
                {
                    InventarioItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoDetalheId = table.Column<int>(type: "int", nullable: false),
                    SkinId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Solicitado = table.Column<bool>(type: "bit", nullable: false),
                    InventarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventarioItens", x => x.InventarioItemId);
                    table.ForeignKey(
                        name: "FK_InventarioItens_Inventarios_InventarioId",
                        column: x => x.InventarioId,
                        principalTable: "Inventarios",
                        principalColumn: "InventarioId");
                    table.ForeignKey(
                        name: "FK_InventarioItens_PedidoDetalhes_PedidoDetalheId",
                        column: x => x.PedidoDetalheId,
                        principalTable: "PedidoDetalhes",
                        principalColumn: "PedidoDetalheId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventarioItens_Skins_SkinId",
                        column: x => x.SkinId,
                        principalTable: "Skins",
                        principalColumn: "SkinId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventarioItens_InventarioId",
                table: "InventarioItens",
                column: "InventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_InventarioItens_PedidoDetalheId",
                table: "InventarioItens",
                column: "PedidoDetalheId");

            migrationBuilder.CreateIndex(
                name: "IX_InventarioItens_SkinId",
                table: "InventarioItens",
                column: "SkinId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventarioItens");

            migrationBuilder.DropTable(
                name: "Inventarios");
        }
    }
}

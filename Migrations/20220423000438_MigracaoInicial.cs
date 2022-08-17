using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkinsSite.Migrations
{
    public partial class MigracaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Skins",
                columns: table => new
                {
                    SkinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    SkinFloat = table.Column<long>(type: "bigint", nullable: false),
                    DescricaoCurta = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescricaoDetalhada = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ImagemUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ImagemThumbnailUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsSkinPreferida = table.Column<bool>(type: "bit", nullable: false),
                    EmEstoque = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skins", x => x.SkinId);
                    table.ForeignKey(
                        name: "FK_Skins_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skins_CategoriaId",
                table: "Skins",
                column: "CategoriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skins");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}

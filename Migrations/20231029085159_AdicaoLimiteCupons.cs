using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkinsSite.Migrations
{
    public partial class AdicaoLimiteCupons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LimiteUso",
                table: "Cupons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UsosCupons",
                columns: table => new
                {
                    UsoCupomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CupomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsosCupons", x => x.UsoCupomId);
                    table.ForeignKey(
                        name: "FK_UsosCupons_Cupons_CupomId",
                        column: x => x.CupomId,
                        principalTable: "Cupons",
                        principalColumn: "CupomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsosCupons_CupomId",
                table: "UsosCupons",
                column: "CupomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsosCupons");

            migrationBuilder.DropColumn(
                name: "LimiteUso",
                table: "Cupons");
        }
    }
}

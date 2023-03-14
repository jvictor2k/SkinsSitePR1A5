using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkinsSite.Migrations
{
    public partial class PopularSkins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Skins(CategoriaId,DescricaoCurta,DescricaoDetalhada,EmEstoque,ImagemThumbnailUrl,ImagemUrl,IsSkinPreferida,Nome,Tipo,Estado,SkinFloat,Preco) " +
                "VALUES(1,'Butterfly Knife Doppler Phase 4','Este é um canivete borboleta. A maior característica deste tipo de arma é a forma como ela se abre, girando livremente em um movimento de rotação que permite um saque rápido.',1,'https://i.ytimg.com/vi/yxbRiK_cttk/maxresdefault.jpg','https://pbs.twimg.com/media/DLk7DI0W4AEN33i.jpg:large',0,'Doppler','Canivete Borboleta','Factory New(Nova de Fábrica)',00015226342657,9000.00)");

            migrationBuilder.Sql("INSERT INTO Skins(CategoriaId,DescricaoCurta,DescricaoDetalhada,EmEstoque,ImagemThumbnailUrl,ImagemUrl,IsSkinPreferida,Nome,Tipo,Estado,SkinFloat,Preco) " +
                "VALUES(1,'Karambit Preto Laminado','Com a sua lâmina curva imitando uma garra de tigre, a Karambit foi criada como parte da tradição marcial de silat do Sudeste Asiático.',1,'https://media.karousell.com/media/photos/products/2020/7/19/csgo_karambit_black_laminate_b_1595141938_1f73a564.jpg','https://i.ytimg.com/vi/2bsMJo_cXh8/maxresdefault.jpg',1,'Preto Laminado','Karambit','Factory New(Nova de Fábrica)',00672921940673,7000.00)");

            migrationBuilder.Sql("INSERT INTO Skins(CategoriaId,DescricaoCurta,DescricaoDetalhada,EmEstoque,ImagemThumbnailUrl,ImagemUrl,IsSkinPreferida,Nome,Tipo,Estado,SkinFloat,Preco) " +
                "VALUES(2,'Luvas Esportivas Caixa de Pandora','O tecido sintético fornece resistência e um visual único a estas luvas atléticas.',1,'https://pbs.twimg.com/media/EDVY5SMUwAA97jM.jpg:large','https://i.imgur.com/eYYwW8yg.jpg',0,'Caixa de Pandora','Luvas esportivas','Battle Scared(Veterana de Guerra)',74489433057492,4500.00)");

            migrationBuilder.Sql("INSERT INTO Skins(CategoriaId,DescricaoCurta,DescricaoDetalhada,EmEstoque,ImagemThumbnailUrl,ImagemUrl,IsSkinPreferida,Nome,Tipo,Estado,SkinFloat,Preco) " +
                "VALUES(2,'Faixas Superimpressão','Preferidas por lutadores corpo a corpo, estas faixas protegem os nós dos dedos e estabilizam o punho ao socar.',1,'https://i.ytimg.com/vi/Kl4frXB4ifo/maxresdefault.jpg','https://arsenalskins.com/wp-content/uploads/2022/01/Design-sem-nome-2022-01-28T232652.450.jpg',0,'Superimpressão','Faixas','Field Tested(Testado em Campo)',16401303046682,2100.00)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Skins");
        }
    }
}

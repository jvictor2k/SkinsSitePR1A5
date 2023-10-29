using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkinsSite.Models;

namespace SkinsSite.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Skin> Skins { get; set; }
        public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }
        public DbSet<Cupom> Cupons { get; set; }
        public DbSet<UsoCupom> UsosCupons { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhes { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<InventarioItem> InventarioItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Config da tabela InventarioItem
            modelBuilder.Entity<InventarioItem>()
                .HasOne(ii => ii.Skin)
                .WithMany()
                .HasForeignKey(ii => ii.SkinId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

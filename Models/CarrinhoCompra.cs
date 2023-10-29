using SkinsSite.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using SkinsSite.Repositories.Interfaces;

namespace SkinsSite.Models
{
    public class CarrinhoCompra
    {
        private readonly AppDbContext _context;

        public CarrinhoCompra(AppDbContext contexto)
        {
            _context = contexto;
        }

        public string CarrinhoCompraId { get; set; }
        public List<CarrinhoCompraItem> CarrinhoCompraItems { get; set; }
        public decimal DescontoTotal { get; set; }
        public List<int> CategoriasCuponsAplicados { get; set; } = new List<int>();
        public List<string> CuponsAplicados { get; set; } = new List<string>();

        public static CarrinhoCompra GetCarrinho(IServiceProvider services)
        {
            //define uma sessao
            ISession session =
                services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            //obtem um servico do tipo do contexto
            var context = services.GetService<AppDbContext>();

            //obtem ou gera o id do carrinho
            string carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();

            //atribui o id do carrinho na sessao
            session.SetString("CarrinhoId", carrinhoId);

            //retorna o carrinho com o contexto e o Id atribuido ou obtido
            return new CarrinhoCompra(context)
            {
                CarrinhoCompraId = carrinhoId
            };
        }

        public bool AdicionarAoCarrinho(Skin skin)
        {
            var skinFromDatabase = _context.Skins.FirstOrDefault(s => s.SkinId == skin.SkinId);

            if (skinFromDatabase != null)
            {
                int limiteEstoque = skinFromDatabase.EmEstoque;

                var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
                s => s.Skin.SkinId == skin.SkinId &&
                s.CarrinhoCompraId == CarrinhoCompraId);

                if (carrinhoCompraItem == null && limiteEstoque > 0)
                {
                    carrinhoCompraItem = new CarrinhoCompraItem
                    {
                        CarrinhoCompraId = CarrinhoCompraId,
                        Skin = skin,
                        Quantidade = 1
                    };
                    _context.CarrinhoCompraItens.Add(carrinhoCompraItem);
                }
                else if (carrinhoCompraItem != null && limiteEstoque > carrinhoCompraItem.Quantidade)
                {
                    carrinhoCompraItem.Quantidade++;
                }
                else
                {
                    return false;
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public int RemoverDoCarrinho(Skin skin)
        {
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
                s => s.Skin.SkinId == skin.SkinId &&
                s.CarrinhoCompraId == CarrinhoCompraId);

            var quantidadelocal = 0;

            if (carrinhoCompraItem != null)
            {
                if (carrinhoCompraItem.Quantidade > 1)
                {
                    carrinhoCompraItem.Quantidade--;
                    quantidadelocal = carrinhoCompraItem.Quantidade;
                }
                else
                {
                    _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
                }
            }
            _context.SaveChanges();
            return quantidadelocal;
        }

        public List<CarrinhoCompraItem> GetCarrinhoCompraItens()
        {
            return CarrinhoCompraItems ??
                (CarrinhoCompraItems =
                    _context.CarrinhoCompraItens
                    .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                    .Include(s => s.Skin)
                    .ToList());
        }

        public void LimparCarrinho()
        {
            var carrinhoItens = _context.CarrinhoCompraItens
                                .Where(carrinho =>
                                carrinho.CarrinhoCompraId == CarrinhoCompraId);

            _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);

            _context.SaveChanges();
        }

        public decimal GetCarrinhoCompraTotal()
        {
            var total = _context.CarrinhoCompraItens
                        .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                        .Select(c => (c.DescontoPreco ?? c.Skin.Preco) * c.Quantidade).Sum();

            return total;
        }
    }
}

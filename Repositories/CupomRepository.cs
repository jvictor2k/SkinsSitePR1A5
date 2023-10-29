using Microsoft.EntityFrameworkCore;
using SkinsSite.Context;
using SkinsSite.Models;
using SkinsSite.Repositories.Interfaces;

namespace SkinsSite.Repositories
{
    public class CupomRepository : ICupomRepository
    {
        private readonly AppDbContext _context;

        public CupomRepository(AppDbContext context)
        {
            _context = context;
        }

        public Cupom ObterCupomPorCodigo(string cupomCodigo)
        {
            return _context.Cupons
                .Include(c => c.Categoria)
                .FirstOrDefault(c => c.CupomCodigo == cupomCodigo);
        }

        public bool VerificaLimiteCupom(Cupom cupom, string userId)
        {
            return cupom.LimiteUso > _context.UsosCupons
                  .Count(uso => uso.CupomId == cupom.CupomId && uso.UserId == userId);
        }

        public IEnumerable<Cupom> Cupons => _context.Cupons;
    }
}

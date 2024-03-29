﻿using SkinsSite.Models;

namespace SkinsSite.Repositories.Interfaces
{
    public interface ICupomRepository
    {
        IEnumerable<Cupom> Cupons { get; }
        Cupom ObterCupomPorCodigo(string cupomCodigo);
        bool VerificaLimiteCupom(Cupom cupom, string userId);
    }
}

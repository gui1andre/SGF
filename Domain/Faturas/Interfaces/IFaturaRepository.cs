using Domain.Faturas.Entities;
using Domain.Faturas.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Faturas.Interfaces
{
    public interface IFaturaRepository
    {
        Task CriarAsync(Fatura fatura);
        Task<IEnumerable<Fatura>> ObterAsync(string? nomeCliente, DateTime? dataInicio, DateTime? dataFim, StatusFatura? status);
        Task<Fatura?> ObterPorIdAsync(Guid id);
        Task AtualizarAsync(Fatura fatura);
        Task DeletarAsync(Fatura fatura);
    }
}

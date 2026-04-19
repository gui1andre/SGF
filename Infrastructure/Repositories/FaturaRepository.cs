using Domain.Faturas.Entities;
using Domain.Faturas.Enums;
using Domain.Faturas.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class FaturaRepository : IFaturaRepository
    {
        private readonly AppDBContext _context;

        public FaturaRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AtualizarAsync(Fatura fatura)
        {
            _context.Faturas.Update(fatura);
            await _context.SaveChangesAsync();
        }

        public async Task CriarAsync(Fatura fatura)
        {
            await _context.Faturas.AddAsync(fatura);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Fatura fatura)
        {
            _context.Faturas.Remove(fatura);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Fatura>> ObterAsync(string? nomeCliente, DateTime? dataInicio, DateTime? dataFim, StatusFatura? status)
        {
            var query = _context.Faturas.Include(f => f.ItensFatura).AsQueryable();

            if (!string.IsNullOrEmpty(nomeCliente))
                query = query.Where(f => f.NomeCliente.Contains(nomeCliente));

            if (dataInicio.HasValue)
                query = query.Where(f => f.DataEmissao >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(f => f.DataEmissao <= dataFim.Value);

            if (status.HasValue)
                query = query.Where(f => f.Status == status.Value);

            return await query.AsNoTracking().OrderByDescending(f => f.DataEmissao).ToListAsync();
        }

        public async Task<Fatura?> ObterPorIdAsync(Guid id)
        {
            return await _context
                .Faturas
                .Include(f => f.ItensFatura)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}

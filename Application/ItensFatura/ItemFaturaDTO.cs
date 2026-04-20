using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ItensFatura
{
    public record ItemFaturaDTO(Guid Id, Guid FaturaId, string Descricao, int Quantidade, decimal ValorUnitario, decimal ValorTotal, string? Justificativa) { }
}

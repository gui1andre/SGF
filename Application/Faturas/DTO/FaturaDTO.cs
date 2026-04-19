using Application.ItensFatura;
using Domain.Faturas.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Faturas.DTO
{
    public record FaturaDTO(
        Guid Id,
        long Numero,
        string NomeCliente,
        DateTime DataEmissao,
        StatusFatura Status,
        Decimal ValorTotal,
        IEnumerable<ItemFaturaDTO> ItensFatura
        );
}

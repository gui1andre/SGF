using Application.ItensFatura;
using Domain.Faturas.Enums;

namespace Application.Faturas.DTO
{
    public record FaturaDTO(
        Guid Id,
        long Numero,
        string NomeCliente,
        DateTime? DataEmissao,
        StatusFatura Status,
        Decimal ValorTotal,
        IEnumerable<ItemFaturaDTO> ItensFatura
        );
}

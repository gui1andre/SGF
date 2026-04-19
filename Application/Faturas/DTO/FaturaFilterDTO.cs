using Domain.Faturas.Enums;


namespace Application.Faturas.DTO
{
    public record FaturaFilterDTO(
    string? NomeCliente,
    DateTime? DataInicial,
    DateTime? DataFinal,
    StatusFatura? Status
        );
}

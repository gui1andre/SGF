namespace Application.Faturas.DTO
{
    public record AdicionarItemDTO(
        string Descricao,
        int Quantidade,
        decimal ValorUnitario,
        string? Justificativa);
}

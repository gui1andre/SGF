namespace Application.ItensFatura
{
    public record ItemFaturaDTO(Guid Id, Guid FaturaId, string Descricao, int Quantidade, decimal ValorUnitario, decimal ValorTotal, string? Justificativa) { }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Faturas.DTO
{
    public record UpdateItemDTO(
        string Descricao,
        int Quantidade,
        decimal ValorUnitario,
        string? Justificativa);
}

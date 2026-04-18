using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ItensFatura
{
    public class ItemFaturaDTO
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal => ValorUnitario * Quantidade;
        public string? Justificativa { get; set; }
    }
}

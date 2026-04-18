using Application.ItensFatura;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Faturas.DTO
{
    public class FaturaDTO
    {
        public Guid Id { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public StatusFaturaDTO Status { get; private set; } = StatusFaturaDTO.Aberta;

        private readonly List<ItemFaturaDTO> ItensFatura = new List<ItemFaturaDTO>();

        public decimal ValorTotal => ItensFatura.Sum(x => x.ValorTotal);
    }
}

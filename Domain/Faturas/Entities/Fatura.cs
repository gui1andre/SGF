using Domain.Faturas.Enums;
using Domain.ItensFatura.Entities;

namespace Domain.Faturas.Entities
{
    public class Fatura
    {
        public Guid Id { get; set; }
        public long Numero { get; private set; }
        public string NomeCliente { get; private set; } = string.Empty;
        public DateTime DataEmissao { get; private set; }
        public StatusFatura Status { get; private set; } = StatusFatura.Aberta;
        public decimal ValorTotal {  get; private set; }
        private readonly List<ItemFatura> _itensFatura = new List<ItemFatura>();
        public IReadOnlyCollection<ItemFatura> ItensFatura => _itensFatura.AsReadOnly();


        protected Fatura() { }

        public Fatura(string nomeCliente)
        {
            Id = Guid.NewGuid();
            AtualizarCliente(nomeCliente);
            ValorTotal = 0;
        }

        public void AtualizarCliente(string nomeCliente)
        {
            if(string.IsNullOrEmpty(nomeCliente))
                throw new ArgumentException("O nome do cliente é obrigatório.");

            NomeCliente = nomeCliente;
        }

        public void AdicionarItem(ItemFatura itemFatura)
        {
            ValidarFaturaAberta();
            _itensFatura.Add(itemFatura);
            RecalcularValorTotal();
        }

        public void RemoverItem(Guid itemId)
        {
            ValidarFaturaAberta();
            var item = _itensFatura.FirstOrDefault(i => i.Id == itemId)
            ?? throw new InvalidOperationException($"Item não encontrado na fatura.");

            _itensFatura.Remove(item);
            RecalcularValorTotal();
        }

        public void AtualizarItem(Guid itemId, string descricao, int quantidade, decimal valorUnitario, string? justificativa)
        {
            ValidarFaturaAberta();
            var item = _itensFatura.FirstOrDefault(i => i.Id == itemId)
            ?? throw new InvalidOperationException($"Item não encontrado na fatura.");

            item.Atualizar(descricao, quantidade, valorUnitario, justificativa);
            RecalcularValorTotal();
        }

        public void FecharFatura()
        {
            if (Status == StatusFatura.Fechada)
                throw new InvalidOperationException("A fatura já está fechada.");

            DataEmissao = DateTime.UtcNow;
            Status = StatusFatura.Fechada;
        }

        private void RecalcularValorTotal()
        {
            ValorTotal = _itensFatura.Sum(item => item.ValorTotal);
        }

        private void ValidarFaturaAberta() 
        {
            if (this.Status == StatusFatura.Fechada)
                throw new InvalidOperationException("Não é possível alterar uma fatura fechada.");
        }

    }
}

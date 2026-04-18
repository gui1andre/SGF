using Domain.Faturas.Enums;
using Domain.ItensFatura.Entities;

namespace Domain.Faturas.Entities
{
    public class Fatura
    {
        public Guid Id { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public StatusFatura Status { get; private set; } = StatusFatura.Aberta;

        private readonly List<ItemFatura> _itensFatura = new List<ItemFatura>();
        public IReadOnlyCollection<ItemFatura> ItensFatura => _itensFatura.AsReadOnly();
        public decimal ValorTotal => _itensFatura.Sum(x => x.ValorTotal);

    
        public void AdicionarItem(ItemFatura itemFatura)
        {
            itemFatura.Validar();

            if(this.Status == StatusFatura.Fechada)
                throw new InvalidOperationException("Não é possível adicionar itens a uma fatura fechada.");

            _itensFatura.Add(itemFatura);
        }

        public void RemoverItem(ItemFatura itemFatura)
        {   
            if (this.Status == StatusFatura.Fechada)
                throw new InvalidOperationException("Não é possível remover itens de uma fatura fechada.");

            _itensFatura.Remove(itemFatura);
        }

        public void FecharFatura()
        {
            if (Status == StatusFatura.Fechada)
                throw new InvalidOperationException("A fatura já está fechada.");

            DataEmissao = DateTime.UtcNow;
            Status = StatusFatura.Fechada;
        }

       
    }
}

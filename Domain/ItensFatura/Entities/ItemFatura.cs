using Domain.Faturas.Entities;

namespace Domain.ItensFatura.Entities
{
    public class ItemFatura
    {
        public Guid Id { get; set; }
        public Guid FaturaId { get; private set; }
        public string Descricao { get; private set; } = string.Empty;
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public decimal ValorTotal { get; private set; }
        public string? Justificativa { get; private set; }

        protected ItemFatura() { }

        public ItemFatura(Guid faturaId, string descricao, int quantidade, decimal valorUnitario, string? justificativa = null) 
        {
            Id = Guid.NewGuid();
            FaturaId = faturaId;
            Atualizar(descricao, quantidade, valorUnitario, justificativa);
        }

        public void Atualizar(string descricao, int quantidade, decimal valorUnitario, string? justificativa)
        {


            if (string.IsNullOrWhiteSpace(descricao) || descricao.Length < 5)
                throw new ArgumentException("Descrição deve ter mais que 5 caracteres.");

            if (valorUnitario <= 0)
                throw new ArgumentException("Valor unitario deve ser maior que zero.");

            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que 0.");

            if (valorUnitario > 1000 && string.IsNullOrWhiteSpace(justificativa))
                throw new ArgumentException("Itens com valor total superior a R$ 1.000,00 exigem justificativa.");

            Descricao = descricao;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorTotal = valorUnitario * quantidade;

            

            Justificativa = string.IsNullOrEmpty(justificativa) ? null : justificativa;
        }
    }
}

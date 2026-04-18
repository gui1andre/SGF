using Domain.Faturas.Entities;
using Domain.ItensFatura.Exceptions;

namespace Domain.ItensFatura.Entities
{
    public class ItemFatura
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal => ValorUnitario * Quantidade;
        public string? Justificativa { get; set; }
        public virtual Fatura Fatura { get; set; }

        public void Validar()
        {
            if (!ValidarJustificativaNecessaria())
                throw new InvalidJustificativaException("Itens com valor total superior a R$ 1.000,00 exigem justificativa.");

            if(!ValidarDescricao())
                throw new InvalidOperationException("Descrição inválida.");
        }

        private bool ValidarDescricao()
        {
            if(string.IsNullOrWhiteSpace(Descricao) || Descricao.Length < 5)
                return false;

            return true;
        }

        public bool ValidarJustificativaNecessaria()
        {
            if (ValorUnitario > 1000 && string.IsNullOrWhiteSpace(Justificativa))
                return false;

            return true;
        }
    }
}

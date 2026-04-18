using Domain.Faturas.Entities;
using Domain.Faturas.Enums;
using Domain.ItensFatura.Entities;
using Domain.ItensFatura.Exceptions;
using SGF.Domain;

namespace SGF.Domain.Test.Faturas
{
    public class FaturaTests
    {
        [Fact]
        public void Deve_Criar_Fatura_Com_Status_Aberta()
        {
            var fatura = new Fatura { NomeCliente = "Cliente Teste" };

            Assert.Equal(StatusFatura.Aberta, fatura.Status);
        }

        [Fact]
        public void Deve_Recalcular_ValorTotal_Automaticamente_Ao_Adicionar_Itens()
        {
            var fatura = new Fatura { NomeCliente = "Dev Pleno" };
            var item1 = new ItemFatura { Quantidade = 2, ValorUnitario = 100, Descricao = "Descricao do item 1" };
            var item2 = new ItemFatura { Quantidade = 1, ValorUnitario = 50, Descricao = "Descrição do item 2" };

            fatura.AdicionarItem(item1);
            fatura.AdicionarItem(item2);

            Assert.Equal(250m, fatura.ValorTotal);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Item_Acima_De_Mil_Nao_Tem_Justificativa()
        {
            var itemInvalido = new ItemFatura
            {
                Quantidade = 1,
                ValorUnitario = 1500,
                Justificativa = ""
            };

            var ex = Assert.Throws<InvalidJustificativaException>(() => itemInvalido.Validar());

            Assert.Equal("Itens com valor total superior a R$ 1.000,00 exigem justificativa.", ex.Message);
}

        [Fact]
        public void Nao_Deve_Permitir_Alterar_Status_De_Fatura_Fechada()
        {

            var fatura = new Fatura { NomeCliente = "Teste Fechamento" };
            fatura.FecharFatura();

            Assert.Throws<InvalidOperationException>(() => fatura.AdicionarItem(new ItemFatura()));
        }

        [Fact]
        public void Nao_Deve_Permitir_Adicionar_Item_Ao_Fechar_Fatura()
        {
            var fatura = new Fatura { NomeCliente = "Teste Adição Item" };
            fatura.FecharFatura();
            Assert.Throws<InvalidOperationException>(() => fatura.AdicionarItem(new ItemFatura()));
        }

        [Fact]
        public void Nao_Deve_Permitir_Remover_Tem_Ao_Fechar_Fatura() {
            var fatura = new Fatura { NomeCliente = "Teste Remoção Item" };
            var item = new ItemFatura { Quantidade = 1, ValorUnitario = 100, Justificativa = "Teste", Descricao = "Descrição do item 1" };
            fatura.AdicionarItem(item);
            fatura.FecharFatura();

            Assert.Throws<InvalidOperationException>(() => fatura.RemoverItem(item));
        }

        [Theory]
        [InlineData(500, 1, true)]
        [InlineData(1500, 1, false)]
        [InlineData(1500, 1, true, "Justificativa válida")]
        public void Deve_Validar_Regra_De_Justificativa_Com_Diferentes_Valores(decimal valor, int qtd, bool esperado, string? justificativa = null)
        {
            var item = new ItemFatura
            {
                Quantidade = qtd,
                ValorUnitario = valor,
                Justificativa = justificativa
            };

            bool resultado = item.ValidarJustificativaNecessaria();

            Assert.Equal(esperado, resultado);
        }
    }
}

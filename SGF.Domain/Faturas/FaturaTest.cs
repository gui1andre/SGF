using Domain.Faturas.Entities;
using Domain.Faturas.Enums;
using Domain.ItensFatura.Entities;
using SGF.Domain;

namespace SGF.Domain.Test.Faturas
{
    public class FaturaTests
    {
        [Fact]
        public void Deve_Criar_Fatura_Com_Status_Aberta()
        {
            var fatura = new Fatura ("Cliente Teste" );

            Assert.Equal(StatusFatura.Aberta, fatura.Status);
        }

        [Fact]
        public void Deve_Recalcular_ValorTotal_Automaticamente_Ao_Adicionar_Itens()
        {
            var fatura = new Fatura ("Dev Pleno");
            var item1 = new ItemFatura(faturaId: fatura.Id ,quantidade: 2, valorUnitario: 100, descricao: "Descricao do item 1");
            var item2 = new ItemFatura (faturaId: fatura.Id, quantidade: 1, valorUnitario: 50, descricao: "Descrição do item 2" );

            fatura.AdicionarItem(item1);
            fatura.AdicionarItem(item2);

            Assert.Equal(250m, fatura.ValorTotal);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Item_Acima_De_Mil_Nao_Tem_Justificativa()
        {

            var ex = Assert.Throws<ArgumentException>(() => new ItemFatura(

                faturaId: Guid.NewGuid(),
                descricao: "Folha A4",
                quantidade: 1,
                valorUnitario: 1500,
                justificativa: ""
            ));

            Assert.Equal("Itens com valor total superior a R$ 1.000,00 exigem justificativa.", ex.Message);
}

        [Fact]
        public void Nao_Deve_Permitir_Alterar_Status_De_Fatura_Fechada()
        {

            var fatura = new Fatura ("Teste Fechamento");
            fatura.FecharFatura();

            Assert.Throws<InvalidOperationException>(() => fatura.AdicionarItem(new ItemFatura(

                faturaId: fatura.Id,
                descricao: "Folha A4",
                quantidade: 1,
                valorUnitario: 900,
                justificativa: ""
            )));
        }

        [Fact]
        public void Nao_Deve_Permitir_Adicionar_Item_Ao_Fechar_Fatura()
        {
            var fatura = new Fatura ("Teste Adição Item");
            fatura.FecharFatura();
            Assert.Throws<InvalidOperationException>(() => fatura.AdicionarItem(new ItemFatura(

                faturaId: fatura.Id,
                descricao: "Folha A4",
                quantidade: 1,
                valorUnitario: 900,
                justificativa: ""
            )));
        }

        [Fact]
        public void Nao_Deve_Permitir_Remover_Tem_Ao_Fechar_Fatura() {
            var fatura = new Fatura ("Teste Remoção Item");
            var item = new ItemFatura (faturaId: fatura.Id, quantidade: 1, valorUnitario: 100, justificativa: "Teste", descricao: "Descrição do item 1" );
            fatura.AdicionarItem(item);
            fatura.FecharFatura();

            Assert.Throws<InvalidOperationException>(() => fatura.RemoverItem(item.Id));
        }
    }
}

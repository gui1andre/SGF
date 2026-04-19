using Domain.ItensFatura.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGF.Domain.Test.ItensFatura
{
    public class ItemFaturaTests
    {
        [Fact]
        public void Deve_Calcular_ValorTotalItem_Com_Sucesso()
        {
            var item = new ItemFatura
            (
                faturaId: Guid.NewGuid(),
                descricao: "Item de Teste",
                quantidade: 5,
                valorUnitario: 10.50m
            );

            Assert.Equal(52.50m, item.ValorTotal);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Descricao_For_Invalida()
        {

            var ex = Assert.Throws<ArgumentException>(() => new ItemFatura
            (
                faturaId: Guid.NewGuid(),
                descricao: "Abc",
                quantidade: 1,
                valorUnitario: 10
            ));
            Assert.Contains("Descrição deve ter mais que 5 caracteres.", ex.Message);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Acima_De_Mil_Sem_Justificativa()
        { 

            var ex = Assert.Throws<ArgumentException>(() => new ItemFatura
            (
                faturaId: Guid.NewGuid(),
                descricao: "Item de Alto Valor",
                quantidade: 1,
                valorUnitario: 1000.01m,
                justificativa: ""
            ));

            Assert.Equal("Itens com valor total superior a R$ 1.000,00 exigem justificativa.", ex.Message);
        }

        [Fact]
        public void Deve_Validar_Com_Sucesso_Quando_Dados_Forem_Corretos()
        {


            var exception = Record.Exception(() => new ItemFatura
            (
                faturaId: Guid.NewGuid(),
                descricao: "Item Válido com Valor Alto",
                quantidade: 2,
                valorUnitario: 600,
                justificativa: "Compra autorizada pelo setor financeiro"
            ));

            Assert.Null(exception); 
        }
    }
}

using Domain.ItensFatura.Entities;
using Domain.ItensFatura.Exceptions;
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
            {
                Quantidade = 5,
                ValorUnitario = 10.50m
            };

            Assert.Equal(52.50m, item.ValorTotal);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Descricao_For_Invalida()
        {

            var item = new ItemFatura
            {
                Descricao = "Abc",
                Quantidade = 1,
                ValorUnitario = 10
            };

            var ex = Assert.Throws<InvalidOperationException>(() => item.Validar());
            Assert.Contains("Descrição inválida.", ex.Message);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Acima_De_Mil_Sem_Justificativa()
        {
            var item = new ItemFatura
            {
                Descricao = "Item de Alto Valor",
                Quantidade = 1,
                ValorUnitario = 1000.01m,
                Justificativa = ""
            };

            var ex = Assert.Throws<InvalidJustificativaException>(() => item.Validar());
            Assert.Equal("Itens com valor total superior a R$ 1.000,00 exigem justificativa.", ex.Message);
        }

        [Fact]
        public void Deve_Validar_Com_Sucesso_Quando_Dados_Forem_Corretos()
        {
            var item = new ItemFatura
            {
                Descricao = "Item Válido com Valor Alto",
                Quantidade = 2,
                ValorUnitario = 600,
                Justificativa = "Compra autorizada pelo setor financeiro"
            };

            var exception = Record.Exception(() => item.Validar());

            Assert.Null(exception); 
        }
    }
}

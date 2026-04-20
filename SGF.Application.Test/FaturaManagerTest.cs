using Application.Faturas;
using Application.Faturas.DTO;
using Domain.Faturas.Entities;
using Domain.Faturas.Enums;
using Domain.Faturas.Interfaces;
using Domain.ItensFatura.Entities;
using Moq;

namespace SGF.Application.Test
{
    public class FaturaManagerTest
    {
        private readonly Mock<IFaturaRepository> _repoMock;
        private readonly FaturaManager _manager;

        public FaturaManagerTest()
        {
            _repoMock = new Mock<IFaturaRepository>();
            _manager = new FaturaManager(_repoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ComDadosValidos_DeveCriarFaturaComStatusAberta()
        {
            // Arrange
            var dto = new CriarFaturaDTO("Maria Oliveira");
            _repoMock.Setup(r => r.CriarAsync(It.IsAny<Fatura>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _manager.CriarAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Maria Oliveira", result.NomeCliente);
            Assert.Equal(StatusFatura.Aberta, result.Status);
            Assert.Equal(0, result.ValorTotal);
            _repoMock.Verify(r => r.CriarAsync(It.IsAny<Fatura>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_FaturaNaoEncontrada_DeveRetornarNull()
        {
            // Arrange
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Fatura?)null);

            // Act
            var result = await _manager.ObterPorIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_FaturaNaoEncontrada_DeveLancarKeyNotFoundException()
        {
            // Arrange
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Fatura?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _manager.AtualizarAsync(Guid.NewGuid(), new AtualizarClienteDTO("Novo Nome")));
        }

        [Fact]
        public async Task UpdateAsync_FaturaFechada_DeveLancarInvalidOperationException()
        {
            // Arrange
            var fatura = new Fatura("João");
            fatura.AdicionarItem(new ItemFatura(fatura.Id, "Produto Teste", 1, 10m, null));
            fatura.FecharFatura();
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(fatura);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _manager.AtualizarAsync(fatura.Id, new AtualizarClienteDTO("Novo Nome")));
            Assert.Contains("fechada", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_FaturaFechada_DeveLancarInvalidOperationException()
        {
            // Arrange
            var fatura = new Fatura("João");
            fatura.AdicionarItem(new ItemFatura(fatura.Id, "Produto Teste", 1, 10m, null));
            fatura.FecharFatura();
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(fatura);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _manager.DeletarAsync(fatura.Id));
            Assert.Contains("fechada", ex.Message);
        }

        [Fact]
        public async Task FecharAsync_FaturaAberta_DeveAlterarStatusParaFechada()
        {
            // Arrange
            var fatura = new Fatura("João");
            fatura.AdicionarItem(new ItemFatura(fatura.Id, "Produto Teste", 1, 10m, null));
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(fatura);
            _repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Fatura>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _manager.FecharFaturaAsync(fatura.Id);

            // Assert
            Assert.Equal(StatusFatura.Fechada, result.Status);
            _repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Fatura>()), Times.Once);
        }

        [Fact]
        public async Task AddItemAsync_FaturaFechada_DeveLancarInvalidOperationException()
        {
            // Arrange
            var fatura = new Fatura("João");
            fatura.AdicionarItem(new ItemFatura(fatura.Id, "Produto Teste", 1, 10m, null));
            fatura.FecharFatura();
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(fatura);
            var itemDto = new AdicionarItemDTO("Produto", 1, 100m, null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _manager.AdicionarItemAsync(fatura.Id, itemDto));
            Assert.Contains("fechada", ex.Message);
        }

        [Fact]
        public async Task AddItemAsync_FaturaAberta_DeveAdicionarItemERecalcularTotal()
        {
            // Arrange
            var fatura = new Fatura("João");
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(fatura);
            _repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Fatura>()))
                .Returns(Task.CompletedTask);
            var itemDto = new AdicionarItemDTO("Produto", 3, 100m, null);

            // Act
            var result = await _manager.AdicionarItemAsync(fatura.Id, itemDto);

            // Assert
            Assert.Equal(300m, result.ValorTotal);
            Assert.Single(result.ItensFatura);
        }

        [Fact]
        public async Task RemoveItemAsync_FaturaAberta_DeveRemoverItemERecalcularTotal()
        {
            // Arrange
            var fatura = new Fatura("João");
            var item = new ItemFatura(fatura.Id, "Produto", 2, 100m, null);
            fatura.AdicionarItem(item);

            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(fatura);
            _repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Fatura>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _manager.RemoverItemAsync(fatura.Id, item.Id);
            // Assert
            Assert.Equal(0, result.ValorTotal);
            Assert.Empty(result.ItensFatura);
        }

        [Fact]
        public async Task DeleteAsync_FaturaAberta_DeveChamarRepositorio()
        {
            // Arrange
            var fatura = new Fatura("João");
            _repoMock.Setup(r => r.ObterPorIdAsync(fatura.Id))
                .ReturnsAsync(fatura);
            _repoMock.Setup(r => r.DeletarAsync(fatura))
                .Returns(Task.CompletedTask);

            // Act
            await _manager.DeletarAsync(fatura.Id);

            // Assert
            _repoMock.Verify(r => r.DeletarAsync(fatura), Times.Once);
        }

        [Fact]
        public async Task FecharAsync_FaturaSemItens_DeveLancarInvalidOperationException()
        {
            // Arrange
            var fatura = new Fatura("João");
            _repoMock.Setup(r => r.ObterPorIdAsync(fatura.Id))
                .ReturnsAsync(fatura);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _manager.FecharFaturaAsync(fatura.Id));
            Assert.Contains("sem itens", ex.Message);
            _repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Fatura>()), Times.Never);
        }

        [Fact]
        public async Task AddItemAsync_FaturaAberta_DeveRetornarMesmoIdDaFatura()
        {
            // Arrange
            var fatura = new Fatura("João");
            _repoMock.Setup(r => r.ObterPorIdAsync(fatura.Id))
                .ReturnsAsync(fatura);
            _repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Fatura>()))
                .Returns(Task.CompletedTask);

            var itemDto = new AdicionarItemDTO("Produto Teste", 1, 50m, null);

            // Act
            var result = await _manager.AdicionarItemAsync(fatura.Id, itemDto);

            // Assert
            Assert.Equal(fatura.Id, result.Id);
            _repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Fatura>()), Times.Once);
        }

        [Fact]
        public async Task UpdateItemAsync_FaturaAberta_DeveAtualizarItemERetornarNovoTotal()
        {
            // Arrange
            var fatura = new Fatura("João");
            var item = new ItemFatura(fatura.Id, "Produto Original", 1, 100m, null);
            fatura.AdicionarItem(item);

            _repoMock.Setup(r => r.ObterPorIdAsync(fatura.Id))
                .ReturnsAsync(fatura);
            _repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Fatura>()))
                .Returns(Task.CompletedTask);

            var request = new AtualizarItemDTO("Produto Atualizado", 2, 150m, null);

            // Act
            var result = await _manager.AtualizarItemAsync(fatura.Id, item.Id, request);

            // Assert
            Assert.Equal(300m, result.ValorTotal);
            var itemAtualizado = Assert.Single(result.ItensFatura);
            Assert.Equal("Produto Atualizado", itemAtualizado.Descricao);
            Assert.Equal(2, itemAtualizado.Quantidade);
            Assert.Equal(150m, itemAtualizado.ValorUnitario);
            _repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Fatura>()), Times.Once);
        }

        [Fact]
        public async Task RemoveItemAsync_FaturaFechada_DeveLancarInvalidOperationException()
        {
            // Arrange
            var fatura = new Fatura("João");
            var item = new ItemFatura(fatura.Id, "Produto", 1, 100m, null);
            fatura.AdicionarItem(item);
            fatura.FecharFatura();

            _repoMock.Setup(r => r.ObterPorIdAsync(fatura.Id))
                .ReturnsAsync(fatura);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _manager.RemoverItemAsync(fatura.Id, item.Id));
            Assert.Contains("fechada", ex.Message);
        }
    }
}

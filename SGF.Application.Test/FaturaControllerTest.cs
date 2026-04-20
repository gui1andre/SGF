using Application.Faturas.DTO;
using Application.Faturas.Ports;
using Domain.Faturas.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SGF.API.Controllers;

namespace SGF.Application.Test
{
    public class FaturaControllerTest
    {
        private readonly Mock<IFaturaManager> _managerMock;
        private readonly Mock<ILogger<FaturaController>> _loggerMock;
        private readonly FaturaController _controller;

        public FaturaControllerTest()
        {
            _managerMock = new Mock<IFaturaManager>();
            _loggerMock = new Mock<ILogger<FaturaController>>();
            _controller = new FaturaController(_managerMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Criar_ComDadosValidos_DeveRetornarCreatedAtActionComIdDaFatura()
        {
            var dto = new CriarFaturaDTO("Cliente Teste");
            var validatorMock = CreateValidValidatorMock<CriarFaturaDTO>();
            var fatura = CreateFaturaDto();

            _managerMock.Setup(m => m.CriarAsync(dto)).ReturnsAsync(fatura);

            var result = await _controller.Criar(dto, validatorMock.Object);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(FaturaController.ObterPorId), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.Equal(fatura.Id, createdResult.RouteValues!["id"]);
            Assert.Same(fatura, createdResult.Value);
        }

        [Fact]
        public async Task Criar_ComDadosInvalidos_DeveRetornarBadRequestESemChamarManager()
        {
            var dto = new CriarFaturaDTO("Cliente Teste");
            var validatorMock = CreateInvalidValidatorMock<CriarFaturaDTO>("NomeCliente", "Nome inválido");

            var result = await _controller.Criar(dto, validatorMock.Object);

            Assert.IsType<BadRequestObjectResult>(result);
            _managerMock.Verify(m => m.CriarAsync(It.IsAny<CriarFaturaDTO>()), Times.Never);
        }

        [Fact]
        public async Task ObterPorId_QuandoNaoEncontrada_DeveRetornarNotFound()
        {
            _managerMock.Setup(m => m.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((FaturaDTO?)null);

            var result = await _controller.ObterPorId(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AdicionarItem_ComDadosValidos_DeveRetornarCreatedAtActionComRouteId()
        {
            var faturaId = Guid.NewGuid();
            var dto = new AdicionarItemDTO("Produto Teste", 2, 50m, null);
            var validatorMock = CreateValidValidatorMock<AdicionarItemDTO>();
            var resultDto = CreateFaturaDto(faturaId);

            _managerMock.Setup(m => m.AdicionarItemAsync(faturaId, dto))
                .ReturnsAsync(resultDto);

            var result = await _controller.AdicionarItem(faturaId, dto, validatorMock.Object);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(FaturaController.ObterPorId), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.Equal(resultDto.Id, createdResult.RouteValues!["id"]);
            Assert.Same(resultDto, createdResult.Value);
        }

        [Fact]
        public async Task AdicionarItem_ComDadosInvalidos_DeveRetornarBadRequestESemChamarManager()
        {
            var faturaId = Guid.NewGuid();
            var dto = new AdicionarItemDTO("Produto Teste", 2, 50m, null);
            var validatorMock = CreateInvalidValidatorMock<AdicionarItemDTO>("Descricao", "Descrição inválida");

            var result = await _controller.AdicionarItem(faturaId, dto, validatorMock.Object);

            Assert.IsType<BadRequestObjectResult>(result);
            _managerMock.Verify(m => m.AdicionarItemAsync(It.IsAny<Guid>(), It.IsAny<AdicionarItemDTO>()), Times.Never);
        }

        private static Mock<IValidator<T>> CreateValidValidatorMock<T>()
        {
            var validatorMock = new Mock<IValidator<T>>();
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            return validatorMock;
        }

        private static Mock<IValidator<T>> CreateInvalidValidatorMock<T>(string propertyName, string message)
        {
            var validatorMock = new Mock<IValidator<T>>();
            var errors = new List<ValidationFailure>
            {
                new ValidationFailure(propertyName, message)
            };

            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(errors));

            return validatorMock;
        }

        private static FaturaDTO CreateFaturaDto(Guid? id = null)
        {
            return new FaturaDTO(
                id ?? Guid.NewGuid(),
                1001,
                "Cliente Teste",
                DateTime.UtcNow,
                StatusFatura.Aberta,
                100m,
                []
            );
        }
    }
}

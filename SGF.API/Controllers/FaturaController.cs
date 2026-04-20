using Application.Faturas.DTO;
using Application.Faturas.Ports;
using Domain.Faturas.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SGF.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FaturaController : ControllerBase
    {
        private readonly IFaturaManager _manager;
        private readonly ILogger<FaturaController> _logger;

        public FaturaController(IFaturaManager manager, ILogger<FaturaController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] CriarFaturaDTO criarFaturaDTO,
            [FromServices] IValidator<CriarFaturaDTO> validator
            )
        {
            var validationResult = await validator.ValidateAsync(criarFaturaDTO);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var fatura = await _manager.CriarAsync(criarFaturaDTO);

            _logger.LogInformation($"Fatura {fatura.Id} criada com sucesso.");

            return CreatedAtAction(nameof(ObterPorId), new { id = fatura.Id }, fatura);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var fatura = await _manager.ObterPorIdAsync(id);

            if (fatura == null) return NotFound();

            return Ok(fatura);
        }

        [HttpGet]
        public async Task<IActionResult> Obter(
            [FromQuery] string? nomeCliente,
            [FromQuery] DateTime? dataInicial,
            [FromQuery] DateTime? dataFinal,
            [FromQuery] StatusFatura? status
            )
        {
            var filter = new FaturaFilterDTO(nomeCliente, dataInicial, dataFinal, status);

            var result = await _manager.ObterAsync(filter);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarClienteDTO request)
        {
            var result = await _manager.AtualizarAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _manager.DeletarAsync(id);
            return NoContent();
        }

        [HttpPut("{id:guid}/Fechar")]
        public async Task<IActionResult> Fechar(Guid id)
        {
            var result = await _manager.FecharFaturaAsync(id);
            return Ok(result);
        }

        [HttpPost("{faturaId:guid}/itens")]
        public async Task<IActionResult> AdicionarItem(Guid faturaId, [FromBody] AdicionarItemDTO request)
        {
            var result = await _manager.AdicionarItemAsync(faturaId, request);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id });
        }

        [HttpPut("{faturaId:guid}/itens/{itensId:guid}")]
        public async Task<IActionResult> AtualizarItem(Guid faturaId, Guid itensId, AtualizarItemDTO request)
        {
            var result = await _manager.UpdateItemAsync(faturaId, itensId, request);
            return Ok(result);
        }

        [HttpDelete("{faturaId:guid}/itens/{itensId:guid}")]
        public async Task<IActionResult> RemoverItem(Guid faturaId, Guid itensId) 
        {
            var result = await _manager.RemoverItemAsync(faturaId, itensId);
            return Ok(result);
        }
    }
}

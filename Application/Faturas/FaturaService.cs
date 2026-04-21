using Application.Faturas.DTO;
using Application.Faturas.Ports;
using Application.ItensFatura;
using Domain.Faturas.Entities;
using Domain.Faturas.Enums;
using Domain.Faturas.Interfaces;
using Domain.ItensFatura.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Faturas
{
    public class FaturaService : IFaturaService
    {
        private readonly IFaturaRepository _faturaRepository;

        public FaturaService(IFaturaRepository faturaRepository)
        {
            this._faturaRepository = faturaRepository;
        }

        public async Task<FaturaDTO> AtualizarAsync(Guid id, AtualizarClienteDTO request)
        {
            var fatura = await ObterFaturaOrThrowAsync(id);
            
            if(fatura.Status == StatusFatura.Fechada)
                throw new InvalidOperationException("Não é possivel atualizar uma fatura fechada.");

            fatura.AtualizarCliente(request.NomeCliente);

            await _faturaRepository.AtualizarAsync(fatura);
            return MapToDto(fatura);
        }

        public async Task<FaturaDTO> CriarAsync(CriarFaturaDTO request)
        {
            var fatura = new Fatura(request.NomeCliente);
            await _faturaRepository.CriarAsync(fatura);
            return MapToDto(fatura);

        }

        public async Task DeletarAsync(Guid id)
        {
            var fatura = await ObterFaturaOrThrowAsync(id);

            if (fatura.Status == StatusFatura.Fechada)
                throw new InvalidOperationException("Não é possivel remover uma fatura fechada.");

            await _faturaRepository.DeletarAsync(fatura);
        }

        public async Task<FaturaDTO> FecharFaturaAsync(Guid id)
        {
            var fatura = await ObterFaturaOrThrowAsync(id);

            if (fatura.Status == StatusFatura.Fechada)
                throw new InvalidOperationException("A fatura já está fechada.");

            fatura.FecharFatura();

            await _faturaRepository.AtualizarAsync(fatura);

            return MapToDto(fatura);

        }

        public async Task<IEnumerable<FaturaDTO>> ObterAsync(FaturaFilterDTO request)
        {
            var faturas = await _faturaRepository.ObterAsync(request.NomeCliente, request.DataInicial, request.DataFinal, request.Status);
            return faturas.Select(MapToDto);
        }

        public async Task<FaturaDTO?> ObterPorIdAsync(Guid id)
        {
            var fatura = await _faturaRepository.ObterPorIdAsync(id);
            return fatura is null ? null : MapToDto(fatura);
        }
        public async Task<FaturaDTO> AdicionarItemAsync(Guid id, AdicionarItemDTO request)
        {
            var fatura = await ObterFaturaOrThrowAsync(id);
            var itemFatura = new ItemFatura(fatura.Id, request.Descricao, request.Quantidade, request.ValorUnitario, request.Justificativa);

            fatura.AdicionarItem(itemFatura);
            await _faturaRepository.AtualizarAsync(fatura);
            return MapToDto(fatura);
        }

        public async Task<FaturaDTO> RemoverItemAsync(Guid faturaId, Guid itemId)
        {
            var fatura = await ObterFaturaOrThrowAsync(faturaId);

            fatura.RemoverItem(itemId);
            await _faturaRepository.AtualizarAsync(fatura);
            return MapToDto(fatura);
        }

        public async Task<FaturaDTO> AtualizarItemAsync(Guid faturaId, Guid ItemId, AtualizarItemDTO request)
        {
            var fatura = await ObterFaturaOrThrowAsync(faturaId);
            fatura.AtualizarItem(ItemId, request.Descricao, request.Quantidade, request.ValorUnitario, request.Justificativa);
            await _faturaRepository.AtualizarAsync(fatura);

            return MapToDto(fatura);

        }


        private async Task<Fatura> ObterFaturaOrThrowAsync(Guid id)
        {
            var fatura = await _faturaRepository.ObterPorIdAsync(id);
            return fatura is null ? throw new KeyNotFoundException($"Fatura não encontrada.") : fatura;
        }

        private static FaturaDTO MapToDto(Fatura fatura)
        {
            return new FaturaDTO(
                fatura.Id,
                fatura.Numero,
                fatura.NomeCliente,
                fatura.DataEmissao,
                fatura.Status,
                fatura.ValorTotal,
                fatura.ItensFatura.Select(i => new ItemFaturaDTO(
                    i.Id,
                    i.FaturaId,
                    i.Descricao,
                    i.Quantidade,
                    i.ValorUnitario,
                    i.ValorTotal,
                    i.Justificativa
                ))
            );
        }
    }
}

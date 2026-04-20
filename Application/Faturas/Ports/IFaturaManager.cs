using Application.Faturas.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Faturas.Ports
{
    public interface IFaturaManager
    {
        Task<FaturaDTO> CriarAsync(CriarFaturaDTO request);
        Task<FaturaDTO?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<FaturaDTO>> ObterAsync(FaturaFilterDTO request);
        Task<FaturaDTO> AtualizarAsync(Guid id, AtualizarClienteDTO request);
        Task DeletarAsync(Guid id);
        Task<FaturaDTO> FecharFaturaAsync(Guid id);
        Task<FaturaDTO> AdicionarItemAsync(Guid id, AdicionarItemDTO request);
        Task<FaturaDTO> UpdateItemAsync(Guid faturaId, Guid ItemId, UpdateItemDTO request);
        Task<FaturaDTO> RemoverItemAsync(Guid faturaId, Guid itemId);


    }
}

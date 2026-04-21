using Application.Faturas.DTO;

namespace Application.Faturas.Ports
{
    public interface IFaturaService
    {
        Task<FaturaDTO> CriarAsync(CriarFaturaDTO request);
        Task<FaturaDTO?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<FaturaDTO>> ObterAsync(FaturaFilterDTO request);
        Task<FaturaDTO> AtualizarAsync(Guid id, AtualizarClienteDTO request);
        Task DeletarAsync(Guid id);
        Task<FaturaDTO> FecharFaturaAsync(Guid id);
        Task<FaturaDTO> AdicionarItemAsync(Guid id, AdicionarItemDTO request);
        Task<FaturaDTO> AtualizarItemAsync(Guid faturaId, Guid ItemId, AtualizarItemDTO request);
        Task<FaturaDTO> RemoverItemAsync(Guid faturaId, Guid itemId);


    }
}

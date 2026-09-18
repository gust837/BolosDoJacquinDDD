using BolosDoJacquin.Domain.Entities;

namespace BolosDoJacquin.Domain.Interfaces
{
    public interface IAvaliacaoRepository
    {
        Task Cadastrar(Avaliacao avaliacao);

        Task Deletar(Guid id);

        Task Atualizar(Guid id, Avaliacao avaliacao);

        Task<List<Avaliacao>> ListarPorProduto(Guid idProduto);

        Task<List<Avaliacao>> ListarPorUsuario(Guid idUsuario);

        Task<List<Avaliacao>> ListarTodasAdmin();

        Task<Avaliacao?> BuscarPorId(Guid id);

        Task AlterarSituacao(Guid id, bool situacao, string motivoOcultacao);

        Task<Avaliacao?> BuscarPorUsuarioEProduto(Guid idUsuario, Guid idProduto);
    }
}

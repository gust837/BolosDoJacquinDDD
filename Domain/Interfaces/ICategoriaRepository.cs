using BolosDoJacquin.Domain.Entities;

namespace BolosDoJacquin.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task Cadastrar(Categoria categoria);

        Task<List<Categoria>> ListarTodas();

        Task Atualizar(Categoria categoria);

        Task<Categoria?> BuscarPorId(Guid id);
        Task Deletar(Guid id);
    }
}

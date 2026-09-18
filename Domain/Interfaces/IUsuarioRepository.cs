using BolosDoJacquin.Domain.Entities;

namespace BolosDoJacquin.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task Cadastrar(Usuario usuario);

        Task Atualizar(Usuario usuario);

        Task<Usuario?> BuscarPorId(Guid id);

        Task<Usuario?> BuscarPorEmail(string email);

        Task<List<Usuario>> ListarTodos();
    }
}

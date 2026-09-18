using BolosDoJacquin.Domain.Entities;
namespace BolosDoJacquin.Domain.Interfaces;

public interface IProdutoRepository
{
    Task Cadastrar(Produto produto);

    Task<Produto?> BuscarPorId(Guid id);

    Task Atualizar(Produto produto);

    Task<List<Produto>> ListarTodos();

    Task<Produto?> BuscarPorNome(string nomeProduto);
    Task Deletar(Guid id);
}

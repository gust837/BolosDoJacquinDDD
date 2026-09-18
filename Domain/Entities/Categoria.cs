using BolosDoJacquin.Domain.Exceptions;

namespace BolosDoJacquin.Domain.Entities;

public class Categoria
{
    
    public Guid IdCategoria { get; private set; }

    public string NomeCategoria { get; private set; }

    private readonly List<Produto> _produtos;
    public virtual IReadOnlyCollection<Produto> Produtos => _produtos.AsReadOnly();

#pragma warning disable CS8618
    protected Categoria() 
    {
        _produtos = new List<Produto>();
    }
#pragma warning restore CS8618

    public Categoria(string nomeCategoria)
    {
        if (string.IsNullOrWhiteSpace(nomeCategoria))
            throw new DomainException("NOME_VAZIO" ,"O nome da categoria nao pode ser vazio");

        IdCategoria = Guid.NewGuid();
        NomeCategoria = nomeCategoria;
        _produtos = new List<Produto>();
    }

    public void AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new DomainException("NOME_VAZIO", "O nome nao pode ficar vazio na atualizacao");

        NomeCategoria = novoNome;
    }
}


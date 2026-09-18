using BolosDoJacquin.Domain.Enums;
using BolosDoJacquin.Domain.Exceptions;

namespace BolosDoJacquin.Domain.Entities;



public class Produto
{
    public Guid IdProduto { get; private set; }
    public string NomeProduto { get; private set; }
    public decimal Preco { get; private set; }
    public string? ImagemUrl { get; private set; }
    public string? DescricaoCurta { get; private set; }
    public string? DescricaoLonga { get; private set; }
    public bool Disponibilidade { get; private set; }
    public SituacaoProduto Situacao { get; private set; }
    public Guid? IdCategoria { get; private set; }

    public virtual Categoria? Categoria { get; private set; }
    private readonly List<Avaliacao> _avaliacoes;
    public virtual IReadOnlyCollection<Avaliacao> Avaliacoes => _avaliacoes.AsReadOnly();

    #pragma warning disable CS8618
        protected Produto() 
        {
            _avaliacoes = new List<Avaliacao>();
        }
    #pragma warning restore CS8618

    public Produto(string nomeProduto, decimal preco, SituacaoProduto situacao, string? descricaoCurta = null, string? descricaoLonga = null ,Guid? idCategoria = null)
    {
        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new DomainException("NOME_VAZIO" ,"O nome não pode ficar vazio na atualização");

        IdProduto = Guid.NewGuid();
        NomeProduto = nomeProduto;
        DescricaoCurta = descricaoCurta;
        DescricaoLonga = descricaoLonga;
        AtualizarPreco(preco);
        Situacao = situacao;
        IdCategoria = idCategoria;
        Disponibilidade = true;
       _avaliacoes = new List<Avaliacao>();
    }

    public void AtualizarPreco(decimal novoPreco)
    {
        if (novoPreco <= 0)
            throw new DomainException("PRECO_INVALIDO","O preço do novo produto deve ser maior que zero!");

        Preco = novoPreco;
    }

    public async Task AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new DomainException("NOME_VAZIO","O nome do produto nao pode ser vazio");

        NomeProduto = novoNome;
    }

    public void EsgotarEstoque()
    {
        Disponibilidade = false;
    }

    public void ReabastecerProduto()
    {
        Disponibilidade = true;
    }

    public void MudarSituacao(SituacaoProduto novaSituacao)
    {
        Situacao = novaSituacao;
    }

    public void AdicionarAvaliacao(Avaliacao novaAvaliacao)
    {
        if (novaAvaliacao == null)
            throw new DomainException("AVALIACAO_INVALIDA", "A avaliação informada não pode ser nula ou vazia.");


        if (Situacao == SituacaoProduto.Inativo)
            throw new DomainException("PRODUTO_INATIVO","Não é possível avaliar um produto inativo.");

        _avaliacoes.Add(novaAvaliacao);
    }

    public void AdicionarImagem(string urlImagem)
    {
        ImagemUrl = urlImagem;
    }

}
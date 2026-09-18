using BolosDoJacquin.Domain.Exceptions;

namespace BolosDoJacquin.Domain.Entities;

public partial class Avaliacao
{
    
    public Guid IdAvaliacao { get; private set; }

    public byte Nota { get; private set; }

    public string? Comentario { get; private set; }

    public bool Situacao { get; private set; }

    public DateTime DataCriacao { get; private set; }

    public DateTime? DataAlteracao { get; private set; }

    public Guid IdUsuario { get; private set; }

    public Guid IdProduto { get; private set; }

    public virtual Produto? Produto { get; private set; }

    public virtual Usuario? Usuario { get; private set; }

    public string? MotivoOcultacao { get; private set; }

    protected Avaliacao() { }

    public Avaliacao(byte nota, string? comentario, Guid idUsuario, Guid idProduto)
    {
        if (nota < 1 || nota > 5)
            throw new DomainException("NOTA_INVALIDA" ,"A nota deve ser entre 1 a 5 estrelas.");

        IdAvaliacao = Guid.NewGuid();
        Nota = nota;
        Comentario = comentario;
        Situacao = true;
        DataCriacao = DateTime.Now;

        IdUsuario = idUsuario;
        IdProduto = idProduto;
    }

    public void AtualizarAvaliacao(byte nota, string? comentario)
    {
        if (nota < 1 || nota > 5)
            throw new DomainException("NOTA_INVALIDA", "A nota deve ser entre 1 a 5 estrelas.");

        Nota = nota;
        Comentario = comentario;
        DataAlteracao = DateTime.Now;
    }

    public void RestaurarAvaliacao()
    {
        Situacao = true;
        MotivoOcultacao = null;
    }

    public void OcultarAvaliacao(string motivo)
    {
        Situacao = false;
        MotivoOcultacao = motivo;
        DataAlteracao = DateTime.Now;
    }
}

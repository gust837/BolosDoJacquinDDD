namespace BolosDoJacquin.Domain.Exceptions;

public class DomainException : Exception
{
    public string Codigo { get; }

    public DomainException(string codigo ,string error) : base(error) 
    {
        Codigo = codigo;
    }
}

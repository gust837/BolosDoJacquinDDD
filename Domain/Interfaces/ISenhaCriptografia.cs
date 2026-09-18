namespace BolosDoJacquin.Domain.Interfaces
{
    public interface ISenhaCriptografia
    {
        string CriptografarSenha(string senhaPura);
        bool VerificarSenha(string senhaPura, string senhaHash);
    }
}
using BolosDoJacquin.Domain.Interfaces;

namespace BolosDoJacquin.Infra.Security
{
    public class BCryptSenhaCriptografia : ISenhaCriptografia
    {
        public string CriptografarSenha(string senhaPura)
        {
            if (string.IsNullOrWhiteSpace(senhaPura))
                throw new ArgumentException("Senha não pode ser vazia.");

            return BCrypt.Net.BCrypt.HashPassword(senhaPura, 12);
        }

        public bool VerificarSenha(string senhaPura, string senhaHash)
        {
            if (string.IsNullOrWhiteSpace(senhaPura) || string.IsNullOrWhiteSpace(senhaHash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(senhaPura, senhaHash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }
    }
}
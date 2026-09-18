using System.Threading.Tasks;

namespace BolosDoJacquin.Domain.Interfaces
{
    public interface IModeracaoTextoService
    {
        Task<bool> ContemConteudoImproprio(string texto);
    }
}
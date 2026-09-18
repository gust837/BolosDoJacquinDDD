namespace BolosDoJacquin.Domain.Interfaces
{
    public interface IUploadImagemService
    {
        Task<string> SubirImagem(IFormFile arquivo);
    }
}
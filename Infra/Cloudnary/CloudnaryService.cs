using BolosDoJacquin.Domain.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BolosDoJacquin.Infra.Cloudinary
{
    public class CloudinaryService : IUploadImagemService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var conta = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new CloudinaryDotNet.Cloudinary(conta);
        }

        public async Task<string> SubirImagem(IFormFile arquivo)
        {
            if (arquivo.Length > 0)
            {
                using var stream = arquivo.OpenReadStream();
                var parametrosUpload = new ImageUploadParams()
                {
                    File = new FileDescription(arquivo.FileName, stream),
                    Folder = "BolosDoJacquin"
                };

                var resultado = await _cloudinary.UploadAsync(parametrosUpload);

                if (resultado.Error != null)
                {
                    throw new ArgumentException($"Erro no Cloudinary: {resultado.Error.Message}");
                }
                return resultado.SecureUrl.ToString();
            }

            throw new ArgumentException("Arquivo de imagem vazio ou inválido.");
        }
    }
}
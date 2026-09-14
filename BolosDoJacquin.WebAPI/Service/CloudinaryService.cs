using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BolosDoJacquin.WebAPI.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string?> UploadImagemAsync(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0) return null;

            using var stream = arquivo.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(arquivo.FileName, stream),
                Folder = "bolos_do_jacquin"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl?.ToString();
        }
    }
}
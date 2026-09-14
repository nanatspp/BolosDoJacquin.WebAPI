using BolosDoJacquin.WebAPI.DTOs;
using BolosDoJacquin.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BolosDoJacquin.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly SightengineModerationService _moderationService;
        private readonly CloudinaryService _cloudinaryService;

        public ProdutoController(
            SightengineModerationService moderationService,
            CloudinaryService cloudinaryService)
        {
            _moderationService = moderationService;
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Cadastrar([FromForm] ProdutoDTO dto)
        {
            if (dto.Arquivo != null && dto.Arquivo.Length > 0)
            {
                // 1. Moderação de imagem por IA
                bool eValida = await _moderationService.ImagemEValidaAsync(dto.Arquivo);
                if (!eValida)
                {
                    return BadRequest("A imagem enviada foi rejeitada pela moderação por conter conteúdo impróprio.");
                }

                // 2. Upload para o Cloudinary
                string? urlImagem = await _cloudinaryService.UploadImagemAsync(dto.Arquivo);
                if (string.IsNullOrEmpty(urlImagem))
                {
                    return BadRequest("Falha ao realizar o upload da imagem no Cloudinary.");
                }

                // Atribui a URL gerada pelo Cloudinary ao modelo do banco
                // produto.ImagemUrl = urlImagem;
            }

            // ... Lógica normal de salvar o produto no banco com o DbContext ...

            return Created("", dto);
        }
    }
}
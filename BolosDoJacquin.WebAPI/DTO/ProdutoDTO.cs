using Microsoft.AspNetCore.Http;

namespace BolosDoJacquin.WebAPI.DTOs
{
    public class ProdutoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string DescricaoCurta { get; set; } = string.Empty;
        public string DescricaoLonga { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public Guid IdCategoria { get; set; }
        public bool Disponivel { get; set; } = true;
        public string Situacao { get; set; } = "Ativo";

        // Campo para receber a foto do formulário
        public IFormFile? Arquivo { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace BolosDoJacquin.WebAPI.DTOs;

public class AvaliacaoDTO
{
    [Required(ErrorMessage = "A Nota do comentário é obrigatória.")]

    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public Guid IdUsuario { get; set; }

    [Required(ErrorMessage = "O produto é obrigatório.")]

    public Guid IdProduto { get; set; }
}
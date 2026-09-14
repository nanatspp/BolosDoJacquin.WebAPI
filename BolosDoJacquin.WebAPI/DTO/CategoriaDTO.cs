using System.ComponentModel.DataAnnotations;

namespace BolosDoJacquin.WebAPI.DTOs;

public class CategoriaDTO
{
    [Required(ErrorMessage = "O Nome é obrigatorio.")]
    [StringLength(100, ErrorMessage = "O Nome deve ter no maximo 100 caracteres")]

    public string Nome { get; set; } = string.Empty;
}
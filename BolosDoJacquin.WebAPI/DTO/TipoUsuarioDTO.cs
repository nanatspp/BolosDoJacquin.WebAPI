using System.ComponentModel.DataAnnotations;

namespace BolosDoJacquin.WebAPI.DTOs;

public class TipoUsuarioDTO
{
    [Required(ErrorMessage = "O titulo é obrigatorio.")]
    [StringLength(100, ErrorMessage = "O titulo deve ter no maximo 100 caracteres")]

    public string Titulo { get; set; } = string.Empty;
}
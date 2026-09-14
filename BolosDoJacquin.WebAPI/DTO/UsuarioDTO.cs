using System.ComponentModel.DataAnnotations;

namespace BolosDoJacquin.WebAPI.DTOs;

public class UsuarioDTO
{
    [Required(ErrorMessage = "Campo obrigatório")]
    [StringLength(100, ErrorMessage = "nOME DEVE ter no maximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "cAMPO Obrigatorio")]
    [EmailAddress(ErrorMessage = "informe um email valido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "campo obrigatorip")]
    [StringLength(60, MinimumLength = 8, ErrorMessage = "senha deve possuir entre 8 e 60 caracteres")]
    public string Senha { get; set; } = string.Empty;
    public Guid IdTipoUsuario { get; set; }
}
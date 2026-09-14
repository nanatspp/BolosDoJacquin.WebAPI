using System.ComponentModel.DataAnnotations;

namespace BolosDoJacquin.WebAPI.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "O e-mail é obrigatório para autenticação")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido")]

    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O senha é obrigatório para autenticação")]
    [StringLength(60, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 60 caracteres")]

    public string Senha { get; set; } = string.Empty;
}
namespace BolosDoJacquin.WebAPI.Models;

public class Usuario
{
    public Guid IdUsuario { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Situacao { get; set; } = "Ativo";
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public Guid IdTipoUsuario { get; set; }
    public TipoUsuario? IdTipoUsuarioNavigation { get; set; }

    public ICollection<Avaliacao> Avaliacao { get; set; } = new List<Avaliacao>();
}
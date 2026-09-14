namespace BolosDoJacquin.WebAPI.Models;

public class TipoUsuario
{
    public Guid IdTipoUsuario { get; set; } = Guid.NewGuid();
    public string Titulo { get; set; } = string.Empty;

    public ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
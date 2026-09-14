namespace BolosDoJacquin.WebAPI.Models;

public class Avaliacao
{
    public Guid IdAvaliacao { get; set; } = Guid.NewGuid();
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public string Situacao { get; set; } = "PUBLICADA";
    public string? MotivoOcultacao { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAlteracao { get; set; } = DateTime.Now;

    public Guid IdUsuario { get; set; }
    public Usuario? IdUsuarioNavigation { get; set; }

    public Guid IdProduto { get; set; }
    public Produto? IdProdutoNavigation { get; set; }
}
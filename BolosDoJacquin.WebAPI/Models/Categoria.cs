namespace BolosDoJacquin.WebAPI.Models;

public class Categoria
{
    public Guid IdCategoria { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;

    public ICollection<Produto> Produto { get; set; } = new List<Produto>();

    // Comentado temporariamente até criarmos o arquivo Produto.cs
    // public ICollection<Produto> Produto { get; set; } = new List<Produto>();
}
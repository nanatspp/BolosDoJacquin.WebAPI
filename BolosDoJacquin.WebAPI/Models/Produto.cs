namespace BolosDoJacquin.WebAPI.Models;

public class Produto
{
    public Guid IdProduto { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string ImagemUrl { get; set; } = string.Empty;
    public string DescricaoCurta { get; set; } = string.Empty;
    public string DescricaoLonga { get; set; } = string.Empty;
    public bool Disponivel { get; set; } = true;
    public string Situacao { get; set; } = "Ativo";

    public Guid IdCategoria { get; set; }
    public Categoria? IdCategoriaNavigation { get; set; }

    public ICollection<Avaliacao> Avaliacao { get; set; } = new List<Avaliacao>();
}
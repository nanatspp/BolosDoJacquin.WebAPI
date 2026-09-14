using BolosDoJacquin.WebAPI.Models;

namespace BolosDoJacquin.WebAPI.Interfaces;

public interface IProduto
{
    Task Cadastrar(Produto produto);
    Task<List<Produto>> Listar();
    Task<Produto?> BuscarPorId(Guid id);
    Task Atualizar(Guid id, Produto produto);
    Task Deletar(Guid id);
}
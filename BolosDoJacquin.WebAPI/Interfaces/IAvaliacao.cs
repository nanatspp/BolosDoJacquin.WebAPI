using BolosDoJacquin.WebAPI.Models;

namespace BolosDoJacquin.WebAPI.Interfaces;

public interface IAvaliacao
{
    Task Cadastrar(Avaliacao avaliacao);
    Task<List<Avaliacao>> Listar();
    Task<List<Avaliacao>> ListarPorProduto(Guid idProduto);
    Task<Avaliacao?> BuscarPorId(Guid id);
    Task Atualizar(Guid id, Avaliacao avaliacao);
    Task Deletar(Guid id);
}
using BolosDoJacquin.WebAPI.Models;

namespace BolosDoJacquin.WebAPI.Interfaces;

public interface IUsuario
{
    Task Cadastrar(Usuario usuario);
    Task<List<Usuario>> Listar();
    Task<Usuario?> BuscarPorId(Guid id);
    Task<Usuario?> BuscarPorEmailESenha(string email, string senha);
    Task Atualizar(Guid id, Usuario usuario);
    Task Deletar(Guid id);
}
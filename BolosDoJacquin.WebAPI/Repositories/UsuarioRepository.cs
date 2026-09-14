using Microsoft.EntityFrameworkCore;
using BolosDoJacquin.WebAPI.BdContext;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Models;

namespace BolosDoJacquin.WebAPI.Repositories;

public class UsuarioRepository : IUsuario
{
    private readonly JacquinContext _context;

    public UsuarioRepository(JacquinContext context)
    {
        _context = context;
    }

    public async Task Cadastrar(Usuario usuario)
    {
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
        await _context.Usuario.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Usuario>> Listar()
    {
        return await _context.Usuario
            .Include(u => u.IdTipoUsuarioNavigation)
            .ToListAsync();
    }

    public async Task<Usuario?> BuscarPorId(Guid id)
    {
        return await _context.Usuario
            .Include(u => u.IdTipoUsuarioNavigation)
            .FirstOrDefaultAsync(u => u.IdUsuario == id);
    }

    public async Task<Usuario?> BuscarPorEmailESenha(string email, string senha)
    {
        var usuario = await _context.Usuario
            .Include(u => u.IdTipoUsuarioNavigation)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (usuario != null && BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
        {
            return usuario;
        }

        return null;
    }

    public async Task Atualizar(Guid id, Usuario usuario)
    {
        var buscado = await _context.Usuario.FindAsync(id);
        if (buscado != null)
        {
            buscado.Nome = usuario.Nome;
            buscado.Email = usuario.Email;
            buscado.Situacao = usuario.Situacao;
            buscado.IdTipoUsuario = usuario.IdTipoUsuario;

            if (!string.IsNullOrEmpty(usuario.SenhaHash))
            {
                buscado.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
            }

            _context.Usuario.Update(buscado);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Deletar(Guid id)
    {
        var buscado = await _context.Usuario.FindAsync(id);
        if (buscado != null)
        {
            _context.Usuario.Remove(buscado);
            await _context.SaveChangesAsync();
        }
    }
}
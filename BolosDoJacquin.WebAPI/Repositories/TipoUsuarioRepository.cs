using Microsoft.EntityFrameworkCore;
using BolosDoJacquin.WebAPI.BdContext;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Models;

namespace BolosDoJacquin.WebAPI.Repositories;

public class TipoUsuarioRepository : ITipoUsuario
{
    private readonly JacquinContext _context;

    public TipoUsuarioRepository(JacquinContext context)
    {
        _context = context;
    }

    public async Task Cadastrar(TipoUsuario tipoUsuario)
    {
        await _context.TipoUsuario.AddAsync(tipoUsuario);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TipoUsuario>> Listar()
    {
        return await _context.TipoUsuario.ToListAsync();
    }

    public async Task<TipoUsuario?> BuscarPorId(Guid id)
    {
        return await _context.TipoUsuario.FirstOrDefaultAsync(t => t.IdTipoUsuario == id);
    }

    public async Task Atualizar(Guid id, TipoUsuario tipoUsuario)
    {
        var buscado = await _context.TipoUsuario.FindAsync(id);
        if (buscado != null)
        {
            buscado.Titulo = tipoUsuario.Titulo;
            _context.TipoUsuario.Update(buscado);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Deletar(Guid id)
    {
        var buscado = await _context.TipoUsuario.FindAsync(id);
        if (buscado != null)
        {
            _context.TipoUsuario.Remove(buscado);
            await _context.SaveChangesAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using BolosDoJacquin.WebAPI.BdContext;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Models;

namespace BolosDoJacquin.WebAPI.Repositories;

public class CategoriaRepository : ICategoria
{
    private readonly JacquinContext _context;

    public CategoriaRepository(JacquinContext context)
    {
        _context = context;
    }

    public async Task Cadastrar(Categoria categoria)
    {
        await _context.Categoria.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Categoria>> Listar()
    {
        return await _context.Categoria.ToListAsync();
    }

    public async Task<Categoria?> BuscarPorId(Guid id)
    {
        return await _context.Categoria.FirstOrDefaultAsync(c => c.IdCategoria == id);
    }

    public async Task Atualizar(Guid id, Categoria categoria)
    {
        var buscada = await _context.Categoria.FindAsync(id);
        if (buscada != null)
        {
            buscada.Nome = categoria.Nome;
            _context.Categoria.Update(buscada);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Deletar(Guid id)
    {
        var buscada = await _context.Categoria.FindAsync(id);
        if (buscada != null)
        {
            _context.Categoria.Remove(buscada);
            await _context.SaveChangesAsync();
        }
    }
}
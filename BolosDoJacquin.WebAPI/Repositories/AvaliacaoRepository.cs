using Microsoft.EntityFrameworkCore;
using BolosDoJacquin.WebAPI.BdContext;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Models;

namespace BolosDoJacquin.WebAPI.Repositories;

public class AvaliacaoRepository : IAvaliacao
{
    private readonly JacquinContext _context;

    public AvaliacaoRepository(JacquinContext context)
    {
        _context = context;
    }

    public async Task Cadastrar(Avaliacao avaliacao)
    {
        await _context.Avaliacao.AddAsync(avaliacao);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Avaliacao>> Listar()
    {
        return await _context.Avaliacao
            .Include(a => a.IdUsuarioNavigation)
            .Include(a => a.IdProdutoNavigation)
            .ToListAsync();
    }

    public async Task<List<Avaliacao>> ListarPorProduto(Guid idProduto)
    {
        return await _context.Avaliacao
            .Include(a => a.IdUsuarioNavigation)
            .Where(a => a.IdProduto == idProduto && a.Situacao == "PUBLICADA")
            .ToListAsync();
    }

    public async Task<Avaliacao?> BuscarPorId(Guid id)
    {
        return await _context.Avaliacao
            .Include(a => a.IdUsuarioNavigation)
            .Include(a => a.IdProdutoNavigation)
            .FirstOrDefaultAsync(a => a.IdAvaliacao == id);
    }

    public async Task Atualizar(Guid id, Avaliacao avaliacao)
    {
        var buscada = await _context.Avaliacao.FindAsync(id);
        if (buscada != null)
        {
            buscada.Nota = avaliacao.Nota;
            buscada.Comentario = avaliacao.Comentario;
            buscada.Situacao = avaliacao.Situacao;
            buscada.MotivoOcultacao = avaliacao.MotivoOcultacao;
            buscada.DataAlteracao = DateTime.Now;

            _context.Avaliacao.Update(buscada);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Deletar(Guid id)
    {
        var buscada = await _context.Avaliacao.FindAsync(id);
        if (buscada != null)
        {
            _context.Avaliacao.Remove(buscada);
            await _context.SaveChangesAsync();
        }
    }
}
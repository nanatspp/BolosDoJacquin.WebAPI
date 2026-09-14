using Microsoft.EntityFrameworkCore;
using BolosDoJacquin.WebAPI.BdContext;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Models;

namespace BolosDoJacquin.WebAPI.Repositories;

public class ProdutoRepository : IProduto
{
    private readonly JacquinContext _context;

    public ProdutoRepository(JacquinContext context)
    {
        _context = context;
    }

    public async Task Cadastrar(Produto produto)
    {
        await _context.Produto.AddAsync(produto);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Produto>> Listar()
    {
        return await _context.Produto
            .Include(p => p.IdCategoriaNavigation)
            .ToListAsync();
    }

    public async Task<Produto?> BuscarPorId(Guid id)
    {
        return await _context.Produto
            .Include(p => p.IdCategoriaNavigation)
            .FirstOrDefaultAsync(p => p.IdProduto == id);
    }

    public async Task Atualizar(Guid id, Produto produto)
    {
        var buscado = await _context.Produto.FindAsync(id);
        if (buscado != null)
        {
            buscado.Nome = produto.Nome;
            buscado.Preco = produto.Preco;
            buscado.ImagemUrl = produto.ImagemUrl;
            buscado.DescricaoCurta = produto.DescricaoCurta;
            buscado.DescricaoLonga = produto.DescricaoLonga;
            buscado.Disponivel = produto.Disponivel;
            buscado.Situacao = produto.Situacao;
            buscado.IdCategoria = produto.IdCategoria;

            _context.Produto.Update(buscado);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Deletar(Guid id)
    {
        var buscado = await _context.Produto.FindAsync(id);
        if (buscado != null)
        {
            _context.Produto.Remove(buscado);
            await _context.SaveChangesAsync();
        }
    }
}
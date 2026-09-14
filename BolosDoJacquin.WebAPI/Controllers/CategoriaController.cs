using BolosDoJacquin.WebAPI.DTOs;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BolosDoJacquin.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly ICategoria _categoriaRepository;

    public CategoriaController(ICategoria categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CategoriaDTO categoriaDto)
    {
        try
        {
            var categoria = new Categoria
            {
                Nome = categoriaDto.Nome
            };

            await _categoriaRepository.Cadastrar(categoria);
            return StatusCode(201, categoria);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var lista = await _categoriaRepository.Listar();
            return Ok(lista);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var categoria = await _categoriaRepository.BuscarPorId(id);
            if (categoria == null) return NotFound("Categoria não encontrada.");

            return Ok(categoria);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, CategoriaDTO categoriaDto)
    {
        try
        {
            var categoria = new Categoria
            {
                Nome = categoriaDto.Nome
            };

            await _categoriaRepository.Atualizar(id, categoria);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _categoriaRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
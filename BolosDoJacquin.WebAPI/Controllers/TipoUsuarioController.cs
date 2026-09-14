using BolosDoJacquin.WebAPI.DTOs;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BolosDoJacquin.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TipoUsuarioController : ControllerBase
{
    private readonly ITipoUsuario _tipoUsuarioRepository;

    public TipoUsuarioController(ITipoUsuario tipoUsuarioRepository)
    {
        _tipoUsuarioRepository = tipoUsuarioRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post(TipoUsuarioDTO tipoUsuarioDto)
    {
        try
        {
            var tipoUsuario = new TipoUsuario
            {
                Titulo = tipoUsuarioDto.Titulo
            };

            await _tipoUsuarioRepository.Cadastrar(tipoUsuario);
            return StatusCode(201, tipoUsuario);
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
            var lista = await _tipoUsuarioRepository.Listar();
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
            var tipoUsuario = await _tipoUsuarioRepository.BuscarPorId(id);
            if (tipoUsuario == null) return NotFound("Tipo de usuário não encontrado.");

            return Ok(tipoUsuario);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, TipoUsuarioDTO tipoUsuarioDto)
    {
        try
        {
            var tipoUsuario = new TipoUsuario
            {
                Titulo = tipoUsuarioDto.Titulo
            };

            await _tipoUsuarioRepository.Atualizar(id, tipoUsuario);
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
            await _tipoUsuarioRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
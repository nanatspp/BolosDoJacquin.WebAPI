using BolosDoJacquin.WebAPI.DTOs;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Models;
using BolosDoJacquin.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BolosDoJacquin.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AvaliacaoController : ControllerBase
{
    private readonly IAvaliacao _avaliacaoRepository;
    private readonly SightengineModerationService _moderationService;

    public AvaliacaoController(
        IAvaliacao avaliacaoRepository,
        SightengineModerationService moderationService)
    {
        _avaliacaoRepository = avaliacaoRepository;
        _moderationService = moderationService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(AvaliacaoDTO avaliacaoDto)
    {
        try
        {
            // 1. Moderação do comentário por IA
            if (!string.IsNullOrWhiteSpace(avaliacaoDto.Comentario))
            {
                bool eValido = await _moderationService.ModerarTextoAsync(avaliacaoDto.Comentario);
                if (!eValido)
                {
                    return BadRequest("O comentário foi rejeitado por conter linguagem imprópria ou ofensiva.");
                }
            }

            // 2. Criação da entidade com IDs e datas preenchidos
            var avaliacao = new Avaliacao
            {
                IdAvaliacao = Guid.NewGuid(),
                Nota = avaliacaoDto.Nota,
                Comentario = avaliacaoDto.Comentario,
                IdUsuario = avaliacaoDto.IdUsuario,
                IdProduto = avaliacaoDto.IdProduto,
                Situacao = "PUBLICADA",
                DataCriacao = DateTime.Now
            };

            await _avaliacaoRepository.Cadastrar(avaliacao);
            return StatusCode(201, avaliacao);
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
            var lista = await _avaliacaoRepository.Listar();
            return Ok(lista);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("produto/{idProduto}")]
    public async Task<IActionResult> GetPorProduto(Guid idProduto)
    {
        try
        {
            var lista = await _avaliacaoRepository.ListarPorProduto(idProduto);
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
            var avaliacao = await _avaliacaoRepository.BuscarPorId(id);
            if (avaliacao == null) return NotFound("Avaliação não encontrada.");

            return Ok(avaliacao);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, AvaliacaoDTO avaliacaoDto)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(avaliacaoDto.Comentario))
            {
                bool eValido = await _moderationService.ModerarTextoAsync(avaliacaoDto.Comentario);
                if (!eValido)
                {
                    return BadRequest("O comentário atualizado foi rejeitado por conter linguagem imprópria ou ofensiva.");
                }
            }

            var avaliacao = new Avaliacao
            {
                Nota = avaliacaoDto.Nota,
                Comentario = avaliacaoDto.Comentario,
                IdUsuario = avaliacaoDto.IdUsuario,
                IdProduto = avaliacaoDto.IdProduto,
                Situacao = "PUBLICADA"
            };

            await _avaliacaoRepository.Atualizar(id, avaliacao);
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
            await _avaliacaoRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
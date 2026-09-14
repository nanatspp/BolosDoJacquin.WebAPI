using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BolosDoJacquin.WebAPI.DTOs;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BolosDoJacquin.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuario _usuarioRepository;
    private readonly IConfiguration _config;

    public UsuarioController(IUsuario usuarioRepository, IConfiguration config)
    {
        _usuarioRepository = usuarioRepository;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Post(UsuarioDTO usuarioDto)
    {
        try
        {
            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                SenhaHash = usuarioDto.Senha,
                IdTipoUsuario = usuarioDto.IdTipoUsuario
            };

            await _usuarioRepository.Cadastrar(usuario);
            return StatusCode(201, new { mensagem = "Usuário cadastrado com sucesso!" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDto)
    {
        try
        {
            var usuario = await _usuarioRepository.BuscarPorEmailESenha(loginDto.Email, loginDto.Senha);
            if (usuario == null) return Unauthorized("E-mail ou senha inválidos.");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.IdTipoUsuarioNavigation?.Titulo ?? "Cliente")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "BolosDoJacquin.WebAPI",
                audience: "BolosDoJacquin.WebAPI",
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
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
            var lista = await _usuarioRepository.Listar();
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
            var usuario = await _usuarioRepository.BuscarPorId(id);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, UsuarioDTO usuarioDto)
    {
        try
        {
            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                SenhaHash = usuarioDto.Senha,
                IdTipoUsuario = usuarioDto.IdTipoUsuario
            };

            await _usuarioRepository.Atualizar(id, usuario);
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
            await _usuarioRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
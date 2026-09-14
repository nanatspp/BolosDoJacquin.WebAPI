using BolosDoJacquin.WebAPI.DTOs;
using BolosDoJacquin.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BolosDoJacquin.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuario _usuarioRepository;
        private readonly IConfiguration _configuration;

        public LoginController(IUsuario usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            try
            {
                // Como o método da interface retorna Task, usamos await:
                var usuarioBuscado = await _usuarioRepository.BuscarPorEmailESenha(loginDto.Email, loginDto.Senha);

                if (usuarioBuscado == null)
                {
                    return NotFound("E-mail ou senha inválidos.");
                }

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.IdUsuario.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                    new Claim(ClaimTypes.Name, usuarioBuscado.Nome),
                    new Claim(ClaimTypes.Role, usuarioBuscado.IdTipoUsuarioNavigation?.Titulo ?? "Cliente")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "BolosDoJacquinChaveSecretaSuperSeguraComMinimo32Caracteres!"));
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
    }
}
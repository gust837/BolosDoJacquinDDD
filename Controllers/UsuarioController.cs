using BolosDoJacquin.Applications.DTOs;
using BolosDoJacquin.Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolosDoJacquin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente([FromBody] CriarUsuarioDTO dto)
        {
            await _usuarioService.CadastrarCliente(dto);
            return Ok("Usuario cadastrado com sucesso.");
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CadastrarAdmin([FromBody] CriarUsuarioDTO dto)
        {
            await _usuarioService.CadastrarAdministrador(dto);
            return Ok("Administrador cadastrado com sucesso.");
        }

        [HttpDelete("{idUsuario}/desativar-conta")]
        public async Task<IActionResult> DesativarConta(Guid idUsuario)
        {
            await _usuarioService.DesativarConta(idUsuario);
            return Ok("Usuario desativado com sucesso.");
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _usuarioService.ListarTodosOsUsuarios();

            return Ok(usuarios);
        }
    }
}

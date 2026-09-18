using BolosDoJacquin.Applications.DTOs;
using BolosDoJacquin.Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolosDoJacquin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "Administrador")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CriarCategoriaDTO dto)
        {
            await _categoriaService.CadastrarNovaCategoria(dto);
            return Ok("Categoria cadastrada com sucesso");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")] // Só o Jacquin apaga categoria
        public async Task<IActionResult> DeletarCategoria(Guid id)
        {
            await _categoriaService.DeletarCategoria(id);
            return Ok("Categoria deletada com sucesso.");
        }

        [HttpPut("{idCategoria}/mudar-nome")]
        public async Task<IActionResult> MudarNome(Guid idCategoria, [FromBody] string novoNome)
        {
            await _categoriaService.AtualizarNome(idCategoria, novoNome);
            return Ok("Nome da categoria atualizado");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ListarCategorias()
        {
            var categorias = await _categoriaService.ListarCategorias();
            return Ok(categorias);
        }
    }
}

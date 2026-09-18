using BolosDoJacquin.Applications.DTOs;
using BolosDoJacquin.Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BolosDoJacquin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CadastrarProduto([FromForm] CriarProdutoDTO dto)
        {
            await _produtoService.CadastrarNovoProduto(dto);

            return Ok("Produto cadastrado com sucesso no cardápio do Jacquin!");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")] // Só o Jacquin apaga produto
        public async Task<IActionResult> DeletarProduto(Guid id)
        {
            await _produtoService.DeletarProduto(id);
            return Ok("Produto deletado com sucesso.");
        }

        [HttpPut("{idProduto}/mudar-preco")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> MudarPreco(Guid idProduto, [FromBody] decimal novoPreco)
        {
            await _produtoService.MudarPreco(idProduto, novoPreco);

            return Ok("Preço atualizado com sucesso!");
        }

        [HttpPut("{idProduto}/mudar-nome")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> MudarNome(Guid idProduto, [FromBody] string novoNome)
        {
            await _produtoService.MudarNome(idProduto, novoNome);

            return Ok("Nome atualizado com sucesso!");
        }

        [HttpGet("catalogo")]
        public async Task<IActionResult> ListarCatalogo()
        {
            var catalogo = await _produtoService.ListarCatalogo();

            return Ok(catalogo);
        }
    }
}

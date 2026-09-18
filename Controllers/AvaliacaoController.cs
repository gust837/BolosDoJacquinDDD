using BolosDoJacquin.Applications.DTOs;
using BolosDoJacquin.Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BolosDoJacquin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly AvaliacaoService _avaliacaoService;

        public AvaliacaoController(AvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarAvaliacao([FromBody] CriarAvaliacaoDTO dto)
        {
            var idUsuarioToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idUsuarioToken))
                return Unauthorized(new{ mensagem = "Token invalido ou usuario nao identificado." });

            var idUsuarioLogado = Guid.Parse(idUsuarioToken);

            var avaliacao = await _avaliacaoService.AdicionarAvaliacao(dto, idUsuarioLogado);

            return CreatedAtAction(nameof(ListarPorProduto), new { idProduto = avaliacao.IdProduto }, avaliacao);
        }

        [HttpGet("produto/{idProduto}")]
        public async Task<IActionResult> ListarPorProduto(Guid idProduto)
        {
            var avaliacoes = await _avaliacaoService.ListarPorProduto(idProduto);
            return Ok(avaliacoes);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> ListarPorUsuario(Guid idUsuario)
        {
            var avaliacoes = await _avaliacaoService.ListarPorUsuario(idUsuario);
            return Ok(avaliacoes);
        }

        [HttpPatch("{id}/ocultar")]
        public async Task<IActionResult> OcultarAvaliacao(Guid id)
        {
            try
            {
                await _avaliacaoService.OcultarAvaliacao(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarAvaliacao(Guid id, [FromBody] AtualizarAvaliacaoDTO dto)
        {
            var idUsuarioToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idUsuarioToken))
                return Unauthorized(new { mensagem = "Token inválido." });

            var idUsuarioLogado = Guid.Parse(idUsuarioToken);

            await _avaliacaoService.EditarAvaliacao(id, dto, idUsuarioLogado);

            return Ok("Sua avaliação foi atualizada com sucesso!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarAvaliacao(Guid id)
        {
            var idUsuarioToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idUsuarioToken))
                return Unauthorized(new {message = "Token inválido."});

            var idUsuarioLogado = Guid.Parse(idUsuarioToken);

            await _avaliacaoService.DeletarAvaliacao(id, idUsuarioLogado);

            return NoContent();
        }
    }
}

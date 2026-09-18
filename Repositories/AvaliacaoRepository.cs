using BolosDoJacquin.BdContextBolos;
using BolosDoJacquin.Domain.Entities;
using BolosDoJacquin.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BolosDoJacquin.Repositories
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        private readonly BolosContext _context;

        public AvaliacaoRepository(BolosContext context)
        {
            _context = context;
        }

        public async Task Cadastrar(Avaliacao avaliacao)
        {
            _context.Avaliacao.Add(avaliacao);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Guid id, Avaliacao avaliacao)
        {
            _context.Avaliacao.Update(avaliacao);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var avaliacao = await BuscarPorId(id);
            if (avaliacao != null)
            {
                _context.Avaliacao.Remove(avaliacao);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Avaliacao?> BuscarPorId(Guid id)
        {
            return await _context.Avaliacao
                .Include(a => a.Usuario)
                .Include(a => a.Produto)
                .FirstOrDefaultAsync(a => a.IdAvaliacao == id);
        }

        public async Task<List<Avaliacao>> ListarPorProduto(Guid idProduto)
        {
            return await _context.Avaliacao
                .Include(a => a.Usuario)
                .Where(a => a.IdProduto == idProduto && a.Situacao == true)
                .ToListAsync();
        }

        public async Task<List<Avaliacao>> ListarPorUsuario(Guid idUsuario)
        {
            return await _context.Avaliacao
                .Include(a => a.Produto)
                .Where(a => a.IdUsuario == idUsuario)
                .ToListAsync();
        }

        public async Task<List<Avaliacao>> ListarTodasAdmin()
        {
            return await _context.Avaliacao
                .Include(a => a.Usuario)
                .Include(a => a.Produto)
                .ToListAsync();
        }

        public async Task AlterarSituacao(Guid id, bool situacao, string motivoOcultacao)
        {
            var avaliacao = await BuscarPorId(id);
            if (avaliacao != null)
            {
                // A entidade tem o método OcultarAvaliacao() para setar false,
                // mas como a interface pede situacao genérica, manipulamos conforme a necessidade.
                if (!situacao)
                {
                    avaliacao.OcultarAvaliacao("Ocultado pelo administrador.");
                }

                _context.Avaliacao.Update(avaliacao);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Avaliacao?> BuscarPorUsuarioEProduto(Guid idUsuario, Guid idProduto)
        {
            return await _context.Avaliacao.FirstOrDefaultAsync(a => a.IdUsuario == idUsuario && a.IdProduto == idProduto);
        }
    }
}

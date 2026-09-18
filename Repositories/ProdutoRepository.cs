using BolosDoJacquin.Applications.Services;
using BolosDoJacquin.BdContextBolos;
using BolosDoJacquin.Domain.Entities;
using BolosDoJacquin.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BolosDoJacquin.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly BolosContext _context;

        public ProdutoRepository(BolosContext context)
        {
            _context = context;
        }

        public async Task Cadastrar(Produto produto)
        {
            await _context.Produto.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<Produto?> BuscarPorId(Guid id)
        {
            return await _context.Produto.FirstOrDefaultAsync(p => p.IdProduto == id);
        }

        public async Task Atualizar(Produto produto)
        {
            _context.Produto.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var obj = await BuscarPorId(id);
            if (obj != null) { _context.Produto.Remove(obj); await _context.SaveChangesAsync(); }
        }

        public async Task<Produto?> BuscarPorNome(string nomeProduto)
        {
            return await _context.Produto.FirstOrDefaultAsync(p => p.NomeProduto == nomeProduto);
        }

        public async Task<List<Produto>> ListarTodos()
        {
            return await _context.Produto.Include(p => p.Categoria).Include(p => p.Avaliacoes).Where(p => p.Situacao == Domain.Enums.SituacaoProduto.Ativo).AsNoTracking().ToListAsync();
        }
    }
}
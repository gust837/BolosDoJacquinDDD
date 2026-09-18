using BolosDoJacquin.BdContextBolos;
using BolosDoJacquin.Domain.Entities;
using BolosDoJacquin.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BolosDoJacquin.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly BolosContext _context;

        public CategoriaRepository(BolosContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Categoria categoria)
        {
            _context.Categoria.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task<Categoria?> BuscarPorId(Guid id)
        {
            return await _context.Categoria.FirstOrDefaultAsync(c => c.IdCategoria == id);
        }

        public async Task Cadastrar(Categoria categoria)
        {
            await _context.Categoria.AddAsync(categoria);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Categoria>> ListarTodas()
        {
            return await _context.Categoria.AsNoTracking().ToListAsync();
        }

        public async Task Deletar(Guid id)
        {
            var obj = await BuscarPorId(id);
            if (obj != null) { _context.Categoria.Remove(obj); await _context.SaveChangesAsync(); }
        }

    }
}

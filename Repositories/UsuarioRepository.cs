using BolosDoJacquin.BdContextBolos;
using BolosDoJacquin.Domain.Entities;
using BolosDoJacquin.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BolosDoJacquin.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly BolosContext _context;

        public UsuarioRepository(BolosContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Usuario usuario)
        { 
            _context.Usuario.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> BuscarPorEmail(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> BuscarPorId(Guid id)
        {
            return await _context.Usuario.FirstOrDefaultAsync(t => t.IdUsuario == id);
        }

        public async Task Cadastrar(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Usuario>> ListarTodos()
        { 
           return _context.Usuario.ToList();
        }
    }
}

using BolosDoJacquin.Applications.DTOs;
using BolosDoJacquin.Domain.Entities;
using BolosDoJacquin.Domain.Exceptions;
using BolosDoJacquin.Domain.Interfaces;
using BolosDoJacquin.Repositories;
using Microsoft.Identity.Client;

namespace BolosDoJacquin.Applications.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task CadastrarNovaCategoria(CriarCategoriaDTO dto)
        {
            var categoria = new Categoria(dto.NomeCategoria);
            await _categoriaRepository.Cadastrar(categoria);
        }

        public async Task AtualizarNome(Guid idCategoria, string novoNome)
        {
            var categoria = await _categoriaRepository.BuscarPorId(idCategoria);

            if (categoria == null)
            {
                throw new NotFoundException("Categoria nao encontrada.");
            }

            categoria.AtualizarNome(novoNome);
            await _categoriaRepository.Atualizar(categoria);
        }

        public async Task DeletarCategoria(Guid idCategoria)
        {
            var categoria = await _categoriaRepository.BuscarPorId(idCategoria);

            if (categoria == null)
                throw new NotFoundException("Categoria não encontrada.");

            try
            {
                await _categoriaRepository.Deletar(idCategoria);
            }
            catch (Exception)
            {
                throw new ConflictException("Não é possível excluir esta categoria, pois já existem produtos vinculados a ela.");
            }
        }

        public async Task<List<Categoria>> ListarCategorias()
        {
            return await _categoriaRepository.ListarTodas();
        }
    }
}

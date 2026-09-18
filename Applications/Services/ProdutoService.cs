using System;
using System.Threading.Tasks;
using BolosDoJacquin.Domain.Entities;
using BolosDoJacquin.Domain.Enums;
using BolosDoJacquin.Applications.DTOs;
using BolosDoJacquin.Domain.Interfaces;
using BolosDoJacquin.Domain.Exceptions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace BolosDoJacquin.Applications.Services;

public class ProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUploadImagemService _uploadImagem;

    public ProdutoService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IUploadImagemService uploadImagem)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _uploadImagem = uploadImagem;
    }

    public async Task CadastrarNovoProduto(CriarProdutoDTO dto)
    {
        var produtoJaExiste = await _produtoRepository.BuscarPorNome(dto.NomeProduto);

        if (produtoJaExiste != null)
            throw new DomainException("NOME_DUPLICADO", "Ja existe um produto com esse nome cadastrado no sistema!");

        if (dto.IdCategoria.HasValue)
        {
            var categoriaExiste = await _categoriaRepository.BuscarPorId(dto.IdCategoria.Value);

            if (categoriaExiste == null)
                throw new NotFoundException("A categoria informada nao existe no sistema!");
        }


        var novoProduto = new Produto(dto.NomeProduto, dto.Preco, SituacaoProduto.Ativo, dto.DescricaoCurta, dto.DescricaoLonga, dto.IdCategoria);

        if (dto.ArquivoDeImagem != null)
        {
            string linkImagem = await _uploadImagem.SubirImagem(dto.ArquivoDeImagem);
            novoProduto.AdicionarImagem(linkImagem);
        }

        await _produtoRepository.Cadastrar(novoProduto);
    }

    public async Task MudarPreco(Guid idProduto, decimal novoPreco)
    {
        var produto = await _produtoRepository.BuscarPorId(idProduto);

        if (produto == null)
            throw new NotFoundException("Bolo não encontrado!");

        produto.AtualizarPreco(novoPreco);

        await _produtoRepository.Atualizar(produto);
    }

    public async Task MudarNome(Guid idProduto, string novoNome)
    {
        var produto = await _produtoRepository.BuscarPorId(idProduto);

        if (produto == null)
            throw new NotFoundException("Produto nao encontrado.");

        var nomeExiste = await _produtoRepository.BuscarPorNome(novoNome);

        if (nomeExiste != null && nomeExiste.IdProduto != idProduto)
            throw new DomainException("NOME_DUPLICADO", "O nome do produto ja existe!");

        await produto.AtualizarNome(novoNome);
        await _produtoRepository.Atualizar(produto);
    }

    public async Task DeletarProduto(Guid id)
    {
        var produto = await _produtoRepository.BuscarPorId(id);

        if (produto == null)
            throw new NotFoundException("Produto não encontrado.");

        try
        {
            await _produtoRepository.Deletar(id);
        }
        catch (DbUpdateException)
        {
            throw new ConflictException("Não é possível excluir este produto, pois ele já possui avaliações vinculadas a ele.");
        }
    }

    public async Task<List<ProdutoResponseDTO>> ListarCatalogo()
    {
        var produtos = await _produtoRepository.ListarTodos();

        var catalogo = produtos.Select(p => new ProdutoResponseDTO
        {
            IdProduto = p.IdProduto,
            NomeProduto = p.NomeProduto,
            Preco = p.Preco,
            ImagemUrl = p.ImagemUrl,
            DescricaoCurta = p.DescricaoCurta,
            DescricaoLonga = p.DescricaoLonga,
            Disponibilidade = p.Disponibilidade,

            NomeCategoria = p.Categoria != null ? p.Categoria.NomeCategoria : "Sem categoria",

            TotalAvaliacoes = p.Avaliacoes.Count(a => a.Situacao == true),
            NotaMedia = p.Avaliacoes.Any(a => a.Situacao == true)
                    ? Math.Round(p.Avaliacoes.Where(a => a.Situacao == true).Average(a => a.Nota), 1) : 0
        }).ToList();

        return catalogo;
    }
}

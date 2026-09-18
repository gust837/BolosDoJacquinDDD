using BolosDoJacquin.Applications.DTOs;
using BolosDoJacquin.Domain.Entities;
using BolosDoJacquin.Domain.Exceptions;
using BolosDoJacquin.Domain.Interfaces;

namespace BolosDoJacquin.Applications.Services
{
    public class AvaliacaoService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IModeracaoTextoService _moderacaoService;

        public AvaliacaoService(
            IAvaliacaoRepository avaliacaoRepository,
            IProdutoRepository produtoRepository,
            IUsuarioRepository usuarioRepository,
            IModeracaoTextoService moderacaoService)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _produtoRepository = produtoRepository;
            _usuarioRepository = usuarioRepository;
            _moderacaoService = moderacaoService;
        }

        public async Task<Avaliacao> AdicionarAvaliacao(CriarAvaliacaoDTO dto, Guid idUsuarioSeguro)
        {
            // Valida se o produto existe
            var produto = await _produtoRepository.BuscarPorId(dto.IdProduto);
            if (produto == null)
            {
                throw new NotFoundException("Produto não encontrado.");
            }

            // Valida se o usuário existe
            var usuario = await _usuarioRepository.BuscarPorId(idUsuarioSeguro);
            if (usuario == null)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            var jaAvaliou = await _avaliacaoRepository.BuscarPorUsuarioEProduto(idUsuarioSeguro, dto.IdProduto);
            if (jaAvaliou != null)
            {
                throw new ConflictException("Você já avaliou este produto. Se desejar, edite a avaliação existente.");
            }

            // Instancia a nova avaliação
            var novaAvaliacao = new Avaliacao(dto.Nota, dto.Comentario, idUsuarioSeguro, dto.IdProduto);

            if (!string.IsNullOrWhiteSpace(dto.Comentario))
            {
                // Manda o comentário para o Sightengine avaliar as violações
                bool conteudoImproprio = await _moderacaoService.ContemConteudoImproprio(dto.Comentario);

                if (conteudoImproprio)
                {
                    // A IA detectou violação! 
                    // Ocultamos a avaliação (Situacao = FALSE), mas NÃO interrompemos o fluxo
                    novaAvaliacao.OcultarAvaliacao("Bloquado automaticamente pela IA de moderação");
                }
            }

            // Salva no banco
            await _avaliacaoRepository.Cadastrar(novaAvaliacao);

            return novaAvaliacao;
        }

        public async Task<List<Avaliacao>> ListarPorProduto(Guid idProduto)
        {
            return await _avaliacaoRepository.ListarPorProduto(idProduto);
        }

        public async Task<List<Avaliacao>> ListarPorUsuario(Guid idUsuario)
        {
            return await _avaliacaoRepository.ListarPorUsuario(idUsuario);
        }

        public async Task OcultarAvaliacao(Guid id)
        {
            await _avaliacaoRepository.AlterarSituacao(id, false, "Ocultado por regra de negócio ou admin.");
        }

        public async Task<Avaliacao> EditarAvaliacao(Guid idAvaliacao, AtualizarAvaliacaoDTO dto, Guid idUsuarioLogado)
        {
            var avaliacao = await _avaliacaoRepository.BuscarPorId(idAvaliacao);
            if (avaliacao == null) throw new NotFoundException("Avaliação não encontrada.");

            // Trava de segurança: só o dono pode editar!
            if (avaliacao.IdUsuario != idUsuarioLogado)
                throw new ForbiddenException("Você não tem permissão para editar a avaliação de outra pessoa.");

            // Atualiza os dados
            avaliacao.AtualizarAvaliacao(dto.Nota, dto.Comentario);
            avaliacao.RestaurarAvaliacao(); // Volta para visível por padrão (Situacao = true)

            // Passa o texto novo no Raio-X da IA de Moderação
            if (!string.IsNullOrWhiteSpace(dto.Comentario))
            {
                bool conteudoImproprio = await _moderacaoService.ContemConteudoImproprio(dto.Comentario);
                if (conteudoImproprio)
                {
                    avaliacao.OcultarAvaliacao("O novo comentário foi bloqueado automaticamente pela IA.");
                }
            }

            await _avaliacaoRepository.Atualizar(idAvaliacao, avaliacao);
            return avaliacao;
        }

        public async Task DeletarAvaliacao(Guid idAvaliacao, Guid idUsuarioLogado)
        {
            var avaliacao = await _avaliacaoRepository.BuscarPorId(idAvaliacao);

            if (avaliacao == null)
                throw new NotFoundException("Avaliação não encontrada");

            if (avaliacao.IdUsuario != idUsuarioLogado)
                throw new ForbiddenException("Você não tem permissão para apagar a avaliação de outra pessoa.");

            await _avaliacaoRepository.Deletar(idAvaliacao);
        }
    }
}

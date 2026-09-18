using BolosDoJacquin.Applications.DTOs;
using BolosDoJacquin.Domain.Entities;
using BolosDoJacquin.Domain.Enums;
using BolosDoJacquin.Domain.Exceptions;
using BolosDoJacquin.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolosDoJacquin.Applications.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _config;
        private readonly TokenService _tokenService;
        private readonly ISenhaCriptografia _senhaCriptografia;

        public UsuarioService(IUsuarioRepository usuarioRepository, IConfiguration config, TokenService tokenService, ISenhaCriptografia senhaCriptografia)
        {
            _usuarioRepository = usuarioRepository;
            _config = config;
            _tokenService = tokenService;
            _senhaCriptografia = senhaCriptografia;
        }

        public async Task<string> FazerLogin(LoginDTO dto)
        {
            var usuario = await _usuarioRepository.BuscarPorEmail(dto.Email);

            if (usuario == null || !_senhaCriptografia.VerificarSenha(dto.Senha, usuario.Senha))
            {
                throw new DomainException("LOGIN_INVALIDO", "Email ou senha incorretos.");
            }

            var chaveSecreta = _config.GetValue<string>("JwtSecret");

            var token = _tokenService.GerarToken(usuario, chaveSecreta!);

            return token;
        }
        
        public async Task CadastrarCliente(CriarUsuarioDTO dto)
        {
            var usuarioExiste = await _usuarioRepository.BuscarPorEmail(dto.Email);

            if (usuarioExiste != null)
            
                throw new DomainException("EMAIL_EXISTENTE" ,"Este e-mail ja esta cadastrado no sistema");

            var novoUsuario = new Usuario(dto.Nome, dto.Email, _senhaCriptografia.CriptografarSenha(dto.Senha), PerfilUsuario.Cliente);

            await _usuarioRepository.Cadastrar(novoUsuario);
        }

        public async Task CadastrarAdministrador(CriarUsuarioDTO dto)
        {
            var usuarioExiste = await _usuarioRepository.BuscarPorEmail(dto.Email);

            if (usuarioExiste != null)
                throw new DomainException("EMAIL_EXISTENTE", "Este e-mail ja esta cadastrado no sistema");

            var novoUsuario = new Usuario(dto.Nome, dto.Email, _senhaCriptografia.CriptografarSenha(dto.Senha), PerfilUsuario.Administrador);

            await _usuarioRepository.Cadastrar(novoUsuario);
        }

        public async Task DesativarConta(Guid idUsuario)
        {
            var usuario = await _usuarioRepository.BuscarPorId(idUsuario);

            if (usuario == null)
                throw new NotFoundException("Usuario nao encontrado");

            usuario.DesativarConta();
            await _usuarioRepository.Atualizar(usuario);
        }

        public async Task<List<Usuario>> ListarTodosOsUsuarios()
        {
            return await _usuarioRepository.ListarTodos();
        }
    }
}

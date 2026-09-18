using BolosDoJacquin.Domain.Enums;
using BolosDoJacquin.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace BolosDoJacquin.Domain.Entities;

public partial class Usuario
{
    public Guid IdUsuario { get; private set; }


    public string Nome { get; private set; }


    public string Email { get; private set; }

    [JsonIgnore]
    public string Senha { get; private set; }


    public PerfilUsuario Perfil { get; private set; }


    public SituacaoUsuario Situacao { get; private set; }

    private readonly List<Avaliacao> _avaliacoes;

    public virtual IReadOnlyCollection<Avaliacao> Avaliacoes => _avaliacoes.AsReadOnly();

    #pragma warning disable CS8618
        protected Usuario() 
        {
            _avaliacoes = new List<Avaliacao>();
        }
    #pragma warning restore CS8618

    public Usuario(string nome, string email, string senha, PerfilUsuario perfil)
    {
        ValidarDados(nome, email, senha);

        IdUsuario = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Senha = senha;
        Perfil = perfil;
        Situacao = SituacaoUsuario.Ativo;
        _avaliacoes = new List<Avaliacao>();
    }



    public void DesativarConta()
    {
        Situacao = SituacaoUsuario.Inativo;
    }

    public void AtualizarDados(string nome, string email, string senha, PerfilUsuario perfil, SituacaoUsuario situacao)
    {
        ValidarDados(nome, email, senha);

        Nome = nome;
        Email = email;
        Senha = senha;
        Perfil = perfil;
        Situacao = situacao;
    }

    private void ValidarDados(string nome, string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("NOME_VAZIO", "O nome de usuario nao pode ser vazio");

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new DomainException("EMAIL_INVALIDO","O email nao pode ser vazio e deve conter @");

        if (string.IsNullOrWhiteSpace(senha) || senha.Length < 8)
            throw new DomainException("SENHA_INVALIDA", "A senha nao pode ser vazia e deve ter no minimo 8 caracteres");
    }
}

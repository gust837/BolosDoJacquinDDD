using BolosDoJacquin.Domain.Entities; // <-- Quando você mover as classes para a pasta Domain, descomente isso!
using Microsoft.EntityFrameworkCore;
using System;

namespace BolosDoJacquin.BdContextBolos;

public class BolosContext : DbContext
{
    // O construtor que recebe as opções (incluindo a string de conexão que virá do Program.cs)
    public BolosContext(DbContextOptions<BolosContext> options) : base(options)
    {
    }

    // Nossas tabelas (DbSets)
    public virtual DbSet<Avaliacao> Avaliacao { get; set; }
    public virtual DbSet<Categoria> Categoria { get; set; }
    public virtual DbSet<Produto> Produto { get; set; }
    public virtual DbSet<Usuario> Usuario { get; set; }

    /* 
       ATENÇÃO: O método OnConfiguring foi APAGADO! 
       A string de conexão não deve ficar aqui. Nós vamos colocar ela no appsettings.json
    */

    // Aqui é onde ensinamos o Entity Framework a criar as tabelas (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Configurando a tabela Categoria
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            // Exemplo: O Nome da categoria é obrigatório e tem no máximo 100 caracteres
            entity.Property(e => e.NomeCategoria)
                  .IsRequired()
                  .HasMaxLength(100)
                  .IsUnicode(false);
        });

        // 2. Configurando a tabela Produto
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.IdProduto);

            entity.Property(e => e.NomeProduto)
                  .IsRequired()
                  .HasMaxLength(100)
                  .IsUnicode(false);

            entity.Property(e => e.Preco)
                  .IsRequired()
                  .HasColumnType("decimal(10, 2)"); // O preço precisa de duas casas decimais

            entity.Property(e => e.ImagemUrl).HasMaxLength(200).IsUnicode(false);
            entity.Property(e => e.DescricaoCurta).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Situacao)
              .IsRequired()
              .HasMaxLength(20)
              .IsUnicode(false)
              .HasConversion<string>();

            entity.Property(e => e.Disponibilidade).HasDefaultValue(true);

            // Chave Estrangeira: Produto pertence a UMA Categoria
            entity.HasOne(d => d.Categoria) 
                  .WithMany(p => p.Produtos) 
                  .HasForeignKey(d => d.IdCategoria)
                  .OnDelete(DeleteBehavior.Restrict); // Regra do SENAI: Não apagar categoria se tiver produto
        });

        // 3. Configurando a tabela Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100).IsUnicode(false);

            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Senha).IsRequired().HasMaxLength(255).IsUnicode(false);

            entity.Property(e => e.Perfil).IsRequired().HasMaxLength(20).IsUnicode(false).HasConversion<string>();
            entity.Property(e => e.Situacao).IsRequired().HasMaxLength(20).IsUnicode(false).HasConversion<string>();
        });

        // 4. Configurando a tabela Avaliacao
        modelBuilder.Entity<Avaliacao>(entity =>
        {
            entity.HasKey(e => e.IdAvaliacao);
            entity.Property(e => e.Nota).IsRequired();
            entity.Property(e => e.Comentario).HasMaxLength(500).IsUnicode(false);
            entity.Property(e => e.DataCriacao).IsRequired();

            // Restrição única (1 usuário só avalia 1 bolo 1 vez)
            entity.HasIndex(e => new { e.IdUsuario, e.IdProduto }).IsUnique();
            // Relações corrigidas com os nomes limpos
            entity.HasOne(d => d.Produto)
                  .WithMany(p => p.Avaliacoes)
                  .HasForeignKey(d => d.IdProduto);
            entity.HasOne(d => d.Usuario)
                  .WithMany(p => p.Avaliacoes)
                  .HasForeignKey(d => d.IdUsuario);
        });
    }
}
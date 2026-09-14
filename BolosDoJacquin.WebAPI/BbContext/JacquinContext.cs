using Microsoft.EntityFrameworkCore;
using BolosDoJacquin.WebAPI.Models;

namespace BolosDoJacquin.WebAPI.BdContext;

public class JacquinContext : DbContext
{
    public JacquinContext(DbContextOptions<JacquinContext> options) : base(options) { }

    public DbSet<TipoUsuario> TipoUsuario { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Produto> Produto { get; set; }
    public DbSet<Avaliacao> Avaliacao { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. TipoUsuario
        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(50);
        });

        // 2. Categoria
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Nome).IsUnique();
        });

        // 3. Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(250);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.SenhaHash).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Situacao).IsRequired().HasMaxLength(20);
            entity.Property(e => e.DataCadastro).HasColumnType("datetime");

            entity.HasOne(d => d.IdTipoUsuarioNavigation)
                .WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdTipoUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 4. Produto
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.IdProduto);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Preco).HasColumnType("decimal(10,2)");
            entity.Property(e => e.ImagemUrl).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DescricaoCurta).IsRequired().HasMaxLength(150);
            entity.Property(e => e.DescricaoLonga).IsRequired();
            entity.Property(e => e.Situacao).IsRequired().HasMaxLength(20);

            entity.HasOne(d => d.IdCategoriaNavigation)
                .WithMany(p => p.Produto)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 5. Avaliacao
        modelBuilder.Entity<Avaliacao>(entity =>
        {
            entity.HasKey(e => e.IdAvaliacao);
            entity.Property(e => e.Nota).IsRequired();
            entity.Property(e => e.Comentario).HasMaxLength(500);
            entity.Property(e => e.Situacao).IsRequired().HasMaxLength(20);
            entity.Property(e => e.MotivoOcultacao).HasMaxLength(255);
            entity.Property(e => e.DataCriacao).HasColumnType("datetime");
            entity.Property(e => e.DataAlteracao).HasColumnType("datetime");

            entity.HasIndex(e => new { e.IdUsuario, e.IdProduto }).IsUnique();

            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.Avaliacao)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.IdProdutoNavigation)
                .WithMany(p => p.Avaliacao)
                .HasForeignKey(d => d.IdProduto)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
using GestaoAcademia.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademia.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Plano> Planos => Set<Plano>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();
    public DbSet<Exercicio> Exercicios => Set<Exercicio>();
    public DbSet<FichaTreino> FichasTreino => Set<FichaTreino>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Herança "table-per-type": Pessoas + Alunos + Usuarios (cada tabela filha
        // tem como PK/FK o IdPessoa). É a tradução direta do triângulo do DER.
        mb.Entity<Pessoa>().ToTable("Pessoas").UseTptMappingStrategy();
        mb.Entity<Aluno>().ToTable("Alunos");
        mb.Entity<Usuario>().ToTable("Usuarios");

        mb.Entity<Aluno>().Property(a => a.StatusAluno).HasConversion<string>();
        mb.Entity<Usuario>().Property(u => u.Perfil).HasConversion<string>();
        mb.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();

        // Restrict = não deixa apagar o "pai" se existirem "filhos" (evita perder dados sem querer)
        mb.Entity<Matricula>(e =>
        {
            e.HasOne(m => m.Aluno).WithMany(a => a.Matriculas)
                .HasForeignKey(m => m.AlunoId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(m => m.Plano).WithMany(p => p.Matriculas)
                .HasForeignKey(m => m.PlanoId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(m => m.Usuario).WithMany(u => u.MatriculasRegistradas)
                .HasForeignKey(m => m.UsuarioId).OnDelete(DeleteBehavior.Restrict);
        });

        mb.Entity<Pagamento>()
            .HasOne(p => p.Matricula).WithMany(m => m.Pagamentos)
            .HasForeignKey(p => p.MatriculaId).OnDelete(DeleteBehavior.Restrict);

        mb.Entity<FichaTreino>(e =>
        {
            e.HasOne(f => f.Aluno).WithMany(a => a.Fichas)
                .HasForeignKey(f => f.AlunoId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(f => f.Usuario).WithMany(u => u.FichasMontadas)
                .HasForeignKey(f => f.UsuarioId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(f => f.Exercicios).WithMany(x => x.Fichas); // N:N
        });
    }
}

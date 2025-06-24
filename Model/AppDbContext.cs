using iTasks;
using iTasks.Model;
using System.Data.Entity;

namespace iTasks
{
    // Classe que representa o contexto da base de dados com Entity Framework
    public class AppDbContext : DbContext
    {
        // Construtor que define a string de ligação à base de dados local (LocalDB)
        public AppDbContext() : base(@"Data Source=(LocalDB)\MSSQLLocalDB;
                               AttachDbFilename=C:\Users\Alexandre\Documents\Projeto_DA\Database.mdf;
                               Integrated Security=True")
        {
            // Esta base de dados será usada durante a execução da aplicação
        }

        // DbSets representam as tabelas na base de dados
        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Gestor> Gestores { get; set; }
        public DbSet<Programador> Programadores { get; set; }
        public DbSet<TipoTarefa> TiposTarefa { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }

        // Método que configura o mapeamento das entidades para tabelas
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utilizador>()
                .HasOptional(u => u.Gestor)
                .WithMany(g => g.Programadores)
                .HasForeignKey(u => u.GestorId);

            // Outros mapeamentos que já tem
            modelBuilder.Entity<Gestor>().ToTable("Gestores");
            modelBuilder.Entity<Programador>().ToTable("Programadores");
            modelBuilder.Entity<Utilizador>().ToTable("Utilizadores");

            base.OnModelCreating(modelBuilder);
        }
    }
}


using iTasks;
using iTasks.Model;
using System.Data.Entity;

namespace iTasks
{
    // Contexto do EF que mapeia as classes do Model às tabelas SQL
    public class AppDbContext : DbContext
    {
        // Connection string usa |DataDirectory| para apontar
        // à pasta de dados do utilizador, tornando o caminho portátil
        public AppDbContext()
            : base(
                @"Data Source=(LocalDB)\MSSQLLocalDB;
                  AttachDbFilename=|DataDirectory|\Database.mdf;
                  Integrated Security=True;
                  Connect Timeout=30"
              )
        {
        }

        // Cada DbSet<T> expõe uma tabela no banco:
        public DbSet<Utilizador> Utilizadores { get; set; }      // tabela base de todos os utilizadores
        public DbSet<Gestor> Gestores { get; set; }              // tabela só dos gestores (TPT)
        public DbSet<Programador> Programadores { get; set; }    // tabela só dos programadores (TPT)
        public DbSet<TipoTarefa> TiposTarefa { get; set; }       // tabela de categorias de tarefa
        public DbSet<Tarefa> Tarefas { get; set; }               // tabela das tarefas em si

        // Aqui configuramos detalhes do mapeamento (fluent API):
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Define o relacionamento opcional 1:N entre Utilizador (programador) e Gestor
            modelBuilder.Entity<Utilizador>()
                .HasOptional(u => u.Gestor)              // cada Utilizador pode ter 0 ou 1 Gestor
                .WithMany(g => g.Programadores)          // cada Gestor tem muitos Programadores
                .HasForeignKey(u => u.GestorId);         // FK na coluna GestorId da tabela Utilizadores

            // Garante que as subclasses ficam em tabelas separadas (TPT)
            modelBuilder.Entity<Gestor>().ToTable("Gestores");
            modelBuilder.Entity<Programador>().ToTable("Programadores");
            modelBuilder.Entity<Utilizador>().ToTable("Utilizadores");

            base.OnModelCreating(modelBuilder);
        }
    }
}
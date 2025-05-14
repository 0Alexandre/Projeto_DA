using iTasks;
using iTasks.Model;
using System.Data.Entity;

namespace iTasks
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\Alexandre\Documents\Projeto_DA\Database.mdf;Integrated Security = True")
        {
        }

        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Gestor> Gestores { get; set; }
        public DbSet<Programador> Programadores { get; set; }
        public DbSet<TipoTarefa> TiposTarefa { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gestor>().ToTable("Gestores");
            modelBuilder.Entity<Programador>().ToTable("Programadores");
            modelBuilder.Entity<Utilizador>().ToTable("Utilizadores");

            base.OnModelCreating(modelBuilder);
        }
    }
}

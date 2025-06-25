using iTasks.Model;
using System.Collections.Generic;
using System.Linq;

namespace iTasks.Controller
{
    public class GestorControlador
    {
        // READ ALL: retorna todos os gestores existentes
        public List<Gestor> ListarGestores()
        {
            using (var db = new AppDbContext())
                // Consulta direta ao DbSet<Gestor>, converte para List
                return db.Gestores.ToList();
        }

        // CREATE: adiciona um novo gestor, garantindo username único
        public bool CriarGestor(Gestor gestor)
        {
            using (var db = new AppDbContext())
            {
                // Verifica se já existe um utilizador com o mesmo username (case-insensitive)
                if (db.Utilizadores.Any(u => u.Username.ToLower() == gestor.Username.ToLower()))
                    return false;                 // falha se username duplicado

                gestor.Tipo = TipoUtilizador.Gestor;    // força o Tipo como Gestor
                db.Utilizadores.Add(gestor);            // TPT: EF adiciona em Utilizadores e em Gestores
                db.SaveChanges();                       // grava alterações no BD
                return true;                            // sucesso
            }
        }

        // READ ONE: obtém um único gestor pelo seu Id (ou null se não existir)
        public Gestor ObterGestorPorId(int id)
        {
            using (var db = new AppDbContext())
                // Usa DbSet<Gestor>.Find para fetch rápido por chave primária
                return db.Gestores.Find(id);
        }

        // UPDATE: atualiza os dados de um gestor existente
        public bool AtualizarGestor(Gestor gestor)
        {
            using (var db = new AppDbContext())
            {
                // Tenta localizar o gestor na BD
                var g = db.Gestores.Find(gestor.Id);
                if (g == null) return false;  // não encontrou → falha

                // Sobreescreve campos editáveis
                g.Nome = gestor.Nome;
                g.Username = gestor.Username;
                g.Password = gestor.Password;
                g.Departamento = gestor.Departamento;
                g.GereUtilizadores = gestor.GereUtilizadores;

                db.SaveChanges();             // grava alterações
                return true;                  // sucesso
            }
        }

        // DELETE: remove um gestor pelo Id
        public bool RemoverGestor(int id)
        {
            using (var db = new AppDbContext())
            {
                // Localiza o gestor
                var g = db.Gestores.Find(id);
                if (g == null)
                    return false;             // não existe → falha

                // Remove o objeto; em TPT, EF apaga em Gestores e em Utilizadores
                db.Utilizadores.Remove(g);
                db.SaveChanges();             // confirma remoção
                return true;                  // sucesso
            }
        }

        // UTILITÁRIO: retorna a lista de departamentos distintos já usados
        public List<string> ObterDepartamentos()
        {
            using (var db = new AppDbContext())
                // Seleciona Departamento de cada Gestor, elimina duplicados
                return db.Gestores
                         .Select(g => g.Departamento)
                         .Distinct()
                         .ToList();
        }
    }
}
using iTasks.Model;
using System.Collections.Generic;
using System.Linq;

namespace iTasks.Controller
{
    public class ProgramadorControlador
    {
        // CREATE: adiciona um novo programador (verifica username único e define tipo)
        public bool CriarProgramador(Programador p)
        {
            using (var db = new AppDbContext())
            {
                // Garante que não existe outro utilizador com o mesmo username (case-insensitive)
                if (db.Utilizadores.Any(u => u.Username.ToLower() == p.Username.ToLower()))
                    return false;   // falha se duplicado

                p.Tipo = TipoUtilizador.Programador;    // define o tipo como Programador
                db.Utilizadores.Add(p);                  // TPT: EF irá inserir em Utilizadores e em Programadores
                db.SaveChanges();                        // grava as alterações no BD
                return true;                             // sucesso
            }
        }

        // READ ALL: obtém lista de todos os programadores
        public List<Programador> ListarProgramadores()
        {
            using (var db = new AppDbContext())
                return db.Programadores.ToList();       // converte o DbSet<Programador> para List
        }

        // READ ONE: obtém um único programador pelo seu Id
        public Programador ObterProgramadorPorId(int id)
        {
            using (var db = new AppDbContext())
                return db.Programadores.Find(id);       // pesquisa por chave primária
        }

        // UPDATE: atualiza os dados de um programador existente
        public bool AtualizarProgramador(Programador prog)
        {
            using (var db = new AppDbContext())
            {
                var p = db.Programadores.Find(prog.Id); // carrega a entidade do BD
                if (p == null)
                    return false;                       // falha se não encontrado

                // Sobrescreve apenas os campos editáveis
                p.Nome = prog.Nome;
                p.Username = prog.Username;
                p.Password = prog.Password;
                p.GestorId = prog.GestorId;       // associa ao gestor
                p.NivelExperiencia = prog.NivelExperiencia;

                db.SaveChanges();                          // grava as alterações
                return true;                               // sucesso
            }
        }

        // DELETE: remove um programador pelo Id
        public bool RemoverProgramador(int id)
        {
            using (var db = new AppDbContext())
            {
                var p = db.Programadores.Find(id);         // localiza o programador
                if (p == null)
                    return false;                          // falha se não existe

                db.Utilizadores.Remove(p);                 // EF remove em Programadores e em Utilizadores (TPT)
                db.SaveChanges();                          // confirma remoção
                return true;                               // sucesso
            }
        }

        // UTILITÁRIO: retorna níveis de experiência distintos já utilizados
        public List<string> ObterNiveisExperiencia()
        {
            using (var db = new AppDbContext())
                return db.Programadores
                         .Select(p => p.NivelExperiencia) // seleciona apenas o campo desejado
                         .Distinct()                     // elimina duplicados
                         .ToList();                      // converte para List
        }
    }
}
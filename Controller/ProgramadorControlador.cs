using iTasks.Model;
using System.Collections.Generic;
using System.Linq;

namespace iTasks.Controller
{
    public class ProgramadorControlador
    {
        // CREATE
        public bool CriarProgramador(Programador p)
        {
            using (var db = new AppDbContext())
            {
                // username único
                if (db.Utilizadores.Any(u => u.Username.ToLower() == p.Username.ToLower()))
                    return false;

                p.Tipo = TipoUtilizador.Programador;
                db.Utilizadores.Add(p);    // TPT insere em Utilizadores e em Programadores
                db.SaveChanges();
                return true;
            }
        }

        // READ ALL
        public List<Programador> ListarProgramadores()
        {
            using (var db = new AppDbContext())
                return db.Programadores.ToList();
        }

        // READ ONE
        public Programador ObterProgramadorPorId(int id)
        {
            using (var db = new AppDbContext())
                return db.Programadores.Find(id);
        }

        // UPDATE
        public bool AtualizarProgramador(Programador prog)
        {
            using (var db = new AppDbContext())
            {
                var p = db.Programadores.Find(prog.Id);
                if (p == null) return false;

                p.Nome = prog.Nome;
                p.Username = prog.Username;
                p.Password = prog.Password;
                p.GestorId = prog.GestorId;
                p.NivelExperiencia = prog.NivelExperiencia;

                db.SaveChanges();
                return true;
            }
        }

        // DELETE
        public bool RemoverProgramador(int id)
        {
            using (var db = new AppDbContext())
            {
                var p = db.Programadores.Find(id);
                if (p == null) return false;

                db.Utilizadores.Remove(p);   // TPT remove de ambas as tabelas
                db.SaveChanges();
                return true;
            }
        }

        // LIST DISTINCT EXPERIENCE LEVELS
        public List<string> ObterNiveisExperiencia()
        {
            using (var db = new AppDbContext())
                return db.Programadores
                         .Select(p => p.NivelExperiencia)
                         .Distinct()
                         .ToList();
        }
    }
}
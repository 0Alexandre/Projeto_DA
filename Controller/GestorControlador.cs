using iTasks.Model;
using System.Collections.Generic;
using System.Linq;

namespace iTasks.Controller
{
    public class GestorControlador
    {
        // READ ALL
        public List<Gestor> ListarGestores()
        {
            using (var db = new AppDbContext())
                return db.Gestores.ToList();
        }

        // CREATE
        public bool CriarGestor(Gestor gestor)
        {
            using (var db = new AppDbContext())
            {
                // username único
                if (db.Utilizadores.Any(u => u.Username.ToLower() == gestor.Username.ToLower()))
                    return false;

                gestor.Tipo = TipoUtilizador.Gestor;
                db.Utilizadores.Add(gestor);  // TPT: EF insere em Utilizadores e Gestores
                db.SaveChanges();
                return true;
            }
        }

        // READ ONE
        public Gestor ObterGestorPorId(int id)
        {
            using (var db = new AppDbContext())
                return db.Gestores.Find(id);
        }

        // UPDATE
        public bool AtualizarGestor(Gestor gestor)
        {
            using (var db = new AppDbContext())
            {
                var g = db.Gestores.Find(gestor.Id);
                if (g == null) return false;

                g.Nome = gestor.Nome;
                g.Username = gestor.Username;
                g.Password = gestor.Password;
                g.Departamento = gestor.Departamento;
                g.GereUtilizadores = gestor.GereUtilizadores;
                db.SaveChanges();
                return true;
            }
        }

        // DELETE
        public bool RemoverGestor(int id)
        {
            using (var db = new AppDbContext())
            {
                var g = db.Gestores.Find(id);
                if (g == null)
                    return false;

                // opcional: antes de remover, mover ou eliminar programadores associados
                db.Utilizadores.Remove(g);  // EF em TPT elimina em Gestores e em Utilizadores
                db.SaveChanges();
                return true;
            }
        }

        // UTILITÁRIO
        public List<string> ObterDepartamentos()
        {
            using (var db = new AppDbContext())
                return db.Gestores
                         .Select(g => g.Departamento)
                         .Distinct()
                         .ToList();
        }
    }
}
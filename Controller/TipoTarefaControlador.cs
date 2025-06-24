using iTasks.Model;
using System.Collections.Generic;
using System.Linq;

namespace iTasks.Controller
{
    public class TipoTarefaControlador
    {
        // Lista todos os tipos de tarefa
        public List<TipoTarefa> ListarTipos()
        {
            using (var db = new AppDbContext())
                return db.TiposTarefa.ToList();
        }

        // Cria um novo tipo (retorna false se já existir)
        public bool CriarTipo(TipoTarefa tipo)
        {
            using (var db = new AppDbContext())
            {
                if (db.TiposTarefa.Any(t => t.Nome.ToLower() == tipo.Nome.ToLower()))
                    return false;
                db.TiposTarefa.Add(tipo);
                db.SaveChanges();
                return true;
            }
        }

        // Atualiza um tipo existente (retorna false se não encontrar)
        public bool AtualizarTipo(TipoTarefa tipo)
        {
            using (var db = new AppDbContext())
            {
                var t = db.TiposTarefa.Find(tipo.Id);
                if (t == null) return false;
                t.Nome = tipo.Nome;
                db.SaveChanges();
                return true;
            }
        }

        // Remove um tipo (retorna false se não encontrar)
        public bool RemoverTipo(int id)
        {
            using (var db = new AppDbContext())
            {
                var t = db.TiposTarefa.Find(id);
                if (t == null) return false;
                db.TiposTarefa.Remove(t);
                db.SaveChanges();
                return true;
            }
        }
    }
}
using iTasks.Model;
using System.Collections.Generic;
using System.Linq;

namespace iTasks.Controller
{
    public class TipoTarefaControlador
    {
        /// <summary>
        /// Lista todos os tipos de tarefa existentes na base de dados.
        /// </summary>
        public List<TipoTarefa> ListarTipos()
        {
            using (var db = new AppDbContext())
                // Retorna todos os registos da tabela TiposTarefa
                return db.TiposTarefa.ToList();
        }

        /// <summary>
        /// Cria um novo tipo de tarefa.
        /// Retorna false se já existir um tipo com o mesmo nome (ignora maiúsculas/minúsculas).
        /// </summary>
        public bool CriarTipo(TipoTarefa tipo)
        {
            using (var db = new AppDbContext())
            {
                // Verifica existência de outro tipo com mesmo nome
                bool existe = db.TiposTarefa
                    .Any(t => t.Nome.ToLower() == tipo.Nome.ToLower());

                if (existe)
                    return false;               // aborta se duplicado

                db.TiposTarefa.Add(tipo);      // adiciona o novo registo
                db.SaveChanges();              // grava no BD
                return true;
            }
        }

        /// <summary>
        /// Atualiza o nome de um tipo de tarefa já existente.
        /// Retorna false se o Id não for encontrado.
        /// </summary>
        public bool AtualizarTipo(TipoTarefa tipo)
        {
            using (var db = new AppDbContext())
            {
                // Procura o registo pelo Id
                var t = db.TiposTarefa.Find(tipo.Id);
                if (t == null)
                    return false;              // não encontrou → aborta

                t.Nome = tipo.Nome;           // altera apenas o campo Nome
                db.SaveChanges();             // grava no BD
                return true;
            }
        }

        /// <summary>
        /// Remove um tipo de tarefa pelo seu Id.
        /// Retorna false se não encontrar esse tipo na base de dados.
        /// </summary>
        public bool RemoverTipo(int id)
        {
            using (var db = new AppDbContext())
            {
                // Procura o registo a remover
                var t = db.TiposTarefa.Find(id);
                if (t == null)
                    return false;              // não encontrado

                db.TiposTarefa.Remove(t);     // marca para remoção
                db.SaveChanges();             // aplica a remoção
                return true;
            }
        }
    }
}
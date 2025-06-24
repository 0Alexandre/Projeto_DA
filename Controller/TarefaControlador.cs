using iTasks.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace iTasks.Controller
{
    public class TarefaControlador
    {
        /// <summary>
        /// Listar tarefas de um gestor por estado (ToDo, Doing, Done)
        /// </summary>
        public List<Tarefa> ListarTarefasPorGestor(int idGestor, string estado)
        {
            using (var db = new AppDbContext())
                return db.Tarefas.Where(t => t.IdGestor == idGestor && t.EstadoAtual == estado).ToList();
        }

        /// <summary>
        /// Listar tarefas de um programador por estado (ToDo, Doing, Done)
        /// </summary>
        public List<Tarefa> ListarTarefasPorProgramador(int idProgramador, string estado)
        {
            using (var db = new AppDbContext())
                return db.Tarefas.Where(t => t.IdProgramador == idProgramador && t.EstadoAtual == estado).ToList();
        }

        /// <summary>
        /// Muda o estado da tarefa e atualiza datas reais (inicio/fim) se aplicável
        /// </summary>
        public bool MudarEstado(int tarefaId, string novoEstado)
        {
            using (var db = new AppDbContext())
            {
                var tarefa = db.Tarefas.Find(tarefaId);
                if (tarefa == null) return false;

                tarefa.EstadoAtual = novoEstado;
                if (novoEstado == "Doing") tarefa.DataRealInicio = DateTime.Now;
                if (novoEstado == "Done") tarefa.DataRealFim = DateTime.Now;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Só permite executar tarefa se o programador tiver menos de 2 Doing
        /// </summary>
        public bool PodeExecutarTarefa(int idProgramador)
        {
            using (var db = new AppDbContext())
                return db.Tarefas.Count(t => t.IdProgramador == idProgramador && t.EstadoAtual == "Doing") < 2;
        }

        /// <summary>
        /// Só permite terminar se for a tarefa Doing com menor ordem
        /// </summary>
        public bool PodeTerminarTarefa(int idProgramador, int ordemExecucao)
        {
            using (var db = new AppDbContext())
            {
                int menorOrdem = db.Tarefas
                    .Where(t => t.IdProgramador == idProgramador && t.EstadoAtual == "Doing")
                    .Select(t => t.OrdemExecucao)
                    .DefaultIfEmpty(int.MaxValue)
                    .Min();

                return ordemExecucao == menorOrdem;
            }
        }

        /// <summary>
        /// Remove tarefa pelo Id (apenas se for do gestor autenticado e ainda estiver em ToDo)
        /// </summary>
        public bool RemoverTarefa(int tarefaId, int idGestor)
        {
            using (var db = new AppDbContext())
            {
                var tarefa = db.Tarefas.Find(tarefaId);
                if (tarefa == null || tarefa.IdGestor != idGestor || tarefa.EstadoAtual != "ToDo")
                    return false;

                db.Tarefas.Remove(tarefa);
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Lista todos os tipos de tarefa
        /// </summary>
        public List<TipoTarefa> ListarTiposTarefa()
        {
            using (var db = new AppDbContext())
                return db.TiposTarefa.ToList();
        }

        /// <summary>
        /// Lista todos os programadores de um gestor
        /// </summary>
        public List<Utilizador> ListarProgramadoresDoGestor(int gestorId)
        {
            using (var db = new AppDbContext())
                return db.Utilizadores
                    .Where(u => u.Tipo == TipoUtilizador.Programador && u.GestorId == gestorId)
                    .ToList();
        }

        /// <summary>
        /// Cria nova tarefa se não existir tarefa com a mesma ordem para o programador
        /// </summary>
        public bool CriarTarefa(Tarefa tarefa)
        {
            using (var db = new AppDbContext())
            {
                bool existe = db.Tarefas.Any(t =>
                    t.IdProgramador == tarefa.IdProgramador &&
                    t.OrdemExecucao == tarefa.OrdemExecucao);

                if (existe) return false;
                db.Tarefas.Add(tarefa);
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Carrega uma tarefa por Id
        /// </summary>
        public Tarefa ObterTarefaPorId(int tarefaId)
        {
            using (var db = new AppDbContext())
                return db.Tarefas.Find(tarefaId);
        }

        /// <summary>
        /// Atualiza dados de uma tarefa já existente
        /// </summary>
        public bool AtualizarTarefa(Tarefa tarefaAtualizada)
        {
            using (var db = new AppDbContext())
            {
                var tarefa = db.Tarefas.Find(tarefaAtualizada.Id);
                if (tarefa == null) return false;

                // Atualiza apenas campos editáveis
                tarefa.Descricao = tarefaAtualizada.Descricao;
                tarefa.IdTipoTarefa = tarefaAtualizada.IdTipoTarefa;
                tarefa.IdProgramador = tarefaAtualizada.IdProgramador;
                tarefa.OrdemExecucao = tarefaAtualizada.OrdemExecucao;
                tarefa.StoryPoints = tarefaAtualizada.StoryPoints;
                tarefa.DataPrevistaInicio = tarefaAtualizada.DataPrevistaInicio;
                tarefa.DataPrevistaFim = tarefaAtualizada.DataPrevistaFim;

                db.SaveChanges();
                return true;
            }
        }

        public bool ExportarConcluidasParaCsv(int idGestor, string filePath)
        {
            using (var db = new AppDbContext())
            {
                // Garante que traz as entidades relacionadas
                db.Configuration.LazyLoadingEnabled = false;
                var tarefas = db.Tarefas
                    .Include("Programador")
                    .Include("TipoTarefa")
                    .Where(t => t.IdGestor == idGestor && t.EstadoAtual == "Done")
                    .ToList();

                using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    sw.WriteLine("Programador;Descricao;DataPrevistaInicio;DataPrevistaFim;TipoTarefa;DataRealInicio;DataRealFim");

                    foreach (var t in tarefas)
                    {
                        // Usa ?.Nome para não explodir se for null
                        var prog = t.Programador?.Nome ?? "";
                        var tipo = t.TipoTarefa?.Nome ?? "";

                        // Datas previstas são DateTime (não-nulas)
                        var pi = t.DataPrevistaInicio.ToString("yyyy-MM-dd");
                        var pf = t.DataPrevistaFim.ToString("yyyy-MM-dd");

                        // Datas reais podem ser null
                        var ri = t.DataRealInicio.HasValue
                                    ? t.DataRealInicio.Value.ToString("yyyy-MM-dd")
                                    : "";
                        var rf = t.DataRealFim.HasValue
                                    ? t.DataRealFim.Value.ToString("yyyy-MM-dd")
                                    : "";

                        sw.WriteLine($"{prog};{t.Descricao};{pi};{pf};{tipo};{ri};{rf}");
                    }
                }
            }
            return true;
        }
    }
}
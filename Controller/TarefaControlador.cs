using iTasks.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity; // for Include(...)
using System.IO;
using System.Linq;
using System.Text;

namespace iTasks.Controller
{
    public class TarefaControlador
    {
        /// <summary>
        /// Lista todas as tarefas de um gestor num dado estado ("ToDo", "Doing" ou "Done").
        /// </summary>
        public List<Tarefa> ListarTarefasPorGestor(int idGestor, string estado)
        {
            using (var db = new AppDbContext())
                // filtra por IdGestor e EstadoAtual
                return db.Tarefas
                         .Where(t => t.IdGestor == idGestor && t.EstadoAtual == estado)
                         .ToList();
        }

        /// <summary>
        /// Lista todas as tarefas de um programador num dado estado.
        /// </summary>
        public List<Tarefa> ListarTarefasPorProgramador(int idProgramador, string estado)
        {
            using (var db = new AppDbContext())
                // filtra por IdProgramador e EstadoAtual
                return db.Tarefas
                         .Where(t => t.IdProgramador == idProgramador && t.EstadoAtual == estado)
                         .ToList();
        }

        /// <summary>
        /// Altera o estado de uma tarefa e define a DataRealInicio ou DataRealFim conforme necessário.
        /// </summary>
        public bool MudarEstado(int tarefaId, string novoEstado)
        {
            using (var db = new AppDbContext())
            {
                var tarefa = db.Tarefas.Find(tarefaId);
                if (tarefa == null) return false;      // não existe

                tarefa.EstadoAtual = novoEstado;
                if (novoEstado == "Doing")
                    tarefa.DataRealInicio = DateTime.Now;
                if (novoEstado == "Done")
                    tarefa.DataRealFim = DateTime.Now;

                db.SaveChanges();                      // grava no BD
                return true;
            }
        }

        /// <summary>
        /// Verifica se o programador já tem 2 tarefas em "Doing" (máximo permitido = 2).
        /// </summary>
        public bool PodeExecutarTarefa(int idProgramador)
        {
            using (var db = new AppDbContext())
                // conta quantas tarefas em Doing
                return db.Tarefas
                         .Count(t => t.IdProgramador == idProgramador && t.EstadoAtual == "Doing")
                       < 2;
        }

        /// <summary>
        /// Só permite terminar (mudar para "Done") a tarefa de menor OrdemExecucao em "Doing".
        /// </summary>
        public bool PodeTerminarTarefa(int idProgramador, int ordemExecucao)
        {
            using (var db = new AppDbContext())
            {
                // obtém a menor ordem atual em Doing
                int menorOrdem = db.Tarefas
                    .Where(t => t.IdProgramador == idProgramador && t.EstadoAtual == "Doing")
                    .Select(t => t.OrdemExecucao)
                    .DefaultIfEmpty(int.MaxValue)
                    .Min();

                // só se a ordem da tarefa for exatamente essa
                return ordemExecucao == menorOrdem;
            }
        }

        /// <summary>
        /// Remove uma tarefa desde que pertença ao gestor e ainda esteja em "ToDo".
        /// </summary>
        public bool RemoverTarefa(int tarefaId, int idGestor)
        {
            using (var db = new AppDbContext())
            {
                var tarefa = db.Tarefas.Find(tarefaId);
                // não existe, pertence a outro gestor ou já saiu de ToDo
                if (tarefa == null
                    || tarefa.IdGestor != idGestor
                    || tarefa.EstadoAtual != "ToDo")
                    return false;

                db.Tarefas.Remove(tarefa);
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Retorna todos os tipos de tarefa para popular os combos.
        /// </summary>
        public List<TipoTarefa> ListarTiposTarefa()
        {
            using (var db = new AppDbContext())
                return db.TiposTarefa.ToList();
        }

        /// <summary>
        /// Lista todos os programadores associados a um determinado gestor.
        /// </summary>
        public List<Utilizador> ListarProgramadoresDoGestor(int gestorId)
        {
            using (var db = new AppDbContext())
                return db.Utilizadores
                         .Where(u => u.Tipo == TipoUtilizador.Programador
                                  && u.GestorId == gestorId)
                         .ToList();
        }

        /// <summary>
        /// Cria nova tarefa, impedindo duplicação de OrdemExecucao por programador.
        /// </summary>
        public bool CriarTarefa(Tarefa tarefa)
        {
            using (var db = new AppDbContext())
            {
                // verifica existência prévia
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
        /// Retorna uma tarefa específica pelo seu Id.
        /// </summary>
        public Tarefa ObterTarefaPorId(int tarefaId)
        {
            using (var db = new AppDbContext())
                return db.Tarefas.Find(tarefaId);
        }

        /// <summary>
        /// Atualiza campos editáveis de uma tarefa já existente.
        /// </summary>
        public bool AtualizarTarefa(Tarefa tarefaAtualizada)
        {
            using (var db = new AppDbContext())
            {
                var tarefa = db.Tarefas.Find(tarefaAtualizada.Id);
                if (tarefa == null) return false;

                // copia apenas os campos que o gestor pode alterar
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

        /// <summary>
        /// Exporta todas as tarefas em "Done" para CSV (ponto e vírgula), incluindo Programador e TipoTarefa.
        /// </summary>
        public bool ExportarConcluidasParaCsv(int idGestor, string filePath)
        {
            using (var db = new AppDbContext())
            {
                // desliga lazy loading para usar Include com strings
                db.Configuration.LazyLoadingEnabled = false;

                var tarefas = db.Tarefas
                    .Include("Programador")
                    .Include("TipoTarefa")
                    .Where(t => t.IdGestor == idGestor && t.EstadoAtual == "Done")
                    .ToList();

                // escreve o ficheiro
                using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    foreach (var t in tarefas)
                    {
                        var prog = t.Programador?.Nome ?? "";
                        var tipo = t.TipoTarefa?.Nome ?? "";

                        // datas previstas (são não-nulas)
                        var pi = t.DataPrevistaInicio.ToString("yyyy-MM-dd");
                        var pf = t.DataPrevistaFim.ToString("yyyy-MM-dd");

                        // datas reais (podem ser null)
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

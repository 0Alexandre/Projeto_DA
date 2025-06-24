using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.Model
{
    public class Tarefa
    {
        public int Id { get; set; }
        public int IdGestor { get; set; }
        public int IdProgramador { get; set; }
        public int IdTipoTarefa { get; set; }
        public int OrdemExecucao { get; set; }
        public string Descricao { get; set; }
        public DateTime DataPrevistaInicio { get; set; }
        public DateTime DataPrevistaFim { get; set; }
        public int StoryPoints { get; set; }
        public DateTime? DataRealInicio { get; set; }
        public DateTime? DataRealFim { get; set; }
        public DateTime DataCriacao { get; set; }
        public string EstadoAtual { get; set; }

        public virtual Gestor Gestor { get; set; }
        public virtual Programador Programador { get; set; }
        public virtual TipoTarefa TipoTarefa { get; set; }

        // Construtor vazio (necessário para Entity Framework)
        public Tarefa() { }

        // Construtor personalizado
        public Tarefa(string descricao, string estado, int storyPoints)
        {
            Descricao = descricao;
            EstadoAtual = estado;
            StoryPoints = storyPoints;
            DataCriacao = DateTime.Now;
        }

        // ToString customizado
        public override string ToString()
        {
            // Podes adaptar o texto apresentado
            return $"{Descricao} | Story Points: {StoryPoints}";
        }
    }
}


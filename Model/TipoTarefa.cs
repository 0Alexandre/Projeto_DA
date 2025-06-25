using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.Model
{
    public class TipoTarefa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<Tarefa> Tarefas { get; set; }

        // Construtor sem parâmetros (necessário ao EF)
        public TipoTarefa()
        {
            Tarefas = new HashSet<Tarefa>();
        }

        // Construtor personalizado para facilitar criação em código
        public TipoTarefa(string nome) : this()
        {
            Nome = nome;
        }

        // ToString para mostrar o nome no UI (combos, listas, debug…)
        public override string ToString()
        {
            return Nome;
        }
    }
}

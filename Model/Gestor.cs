using System;
using System.Collections.Generic;

namespace iTasks.Model
{
    public class Gestor : Utilizador
    {
        public string Departamento { get; set; }
        public bool GereUtilizadores { get; set; }
        public virtual ICollection<Programador> Programadores { get; set; }

        // Construtor vazio (necessário para o Entity Framework)
        public Gestor()
            : base() // chama o construtor vazio de Utilizador
        {
            Programadores = new List<Programador>();
            Tipo = TipoUtilizador.Gestor;
        }

        // Construtor personalizado
        public Gestor(
            string nome,
            string username,
            string password,
            string departamento,
            bool gereUtilizadores
        ) : base(nome, username, password, TipoUtilizador.Gestor)
        {
            Departamento = departamento;
            GereUtilizadores = gereUtilizadores;
            Programadores = new List<Programador>();
        }

        // ToString para apresentar de forma útil na UI e no debug
        public override string ToString()
        {
            // Exemplo: "Ana Santos (Gestor – TI)"
            return $"{Nome} (Gestor – {Departamento})";
        }
    }
}
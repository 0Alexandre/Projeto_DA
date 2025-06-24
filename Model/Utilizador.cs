using System;
using System.Collections.Generic;

namespace iTasks
{
    public enum TipoUtilizador
    {
        Programador = 1,
        Gestor = 2
    }

    // Classe que representa um Utilizador da aplicação
    public class Utilizador
    {
        // Identificador único do utilizador
        public int Id { get; set; }

        // Nome completo do utilizador
        public string Nome { get; set; }

        // Nome de utilizador (usado no login)
        public string Username { get; set; }

        // Palavra-passe do utilizador
        public string Password { get; set; }

        public TipoUtilizador Tipo { get; set; }

        // Se for Programador, deve ter um Gestor
        public int? GestorId { get; set; }
        public virtual Utilizador Gestor { get; set; }

        // Se for Gestor, pode ter vários Programadores
        public virtual ICollection<Utilizador> Programadores { get; set; }

        // Construtor vazio (necessário para EF e uso geral)
        public Utilizador()
        {
            Programadores = new List<Utilizador>();
        }

        // Construtor personalizado (útil para criar rapidamente um utilizador)
        public Utilizador(string nome, string username, string password, TipoUtilizador tipo)
        {
            Nome = nome;
            Username = username;
            Password = password;
            Tipo = tipo;
            Programadores = new List<Utilizador>();
        }

        // ToString para apresentar de forma útil nas listas e debug
        public override string ToString()
        {
            return $"{Nome} ({Tipo})";
        }
    }

    // Classe estática para armazenar a sessão do utilizador atualmente autenticado
    public static class Sessao
    {
        // Propriedade estática que guarda o utilizador que fez login
        public static Utilizador UtilizadorGuardado { get; set; }
    }
}
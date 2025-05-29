using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks
{
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
    }

    // Classe estática para armazenar a sessão do utilizador atualmente autenticado
    public static class Sessao
    {
        // Propriedade estática que guarda o utilizador que fez login
        public static Utilizador UtilizadorGuardado { get; set; }
    }
}


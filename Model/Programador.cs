using System;

namespace iTasks.Model
{
    public class Programador : Utilizador
    {
        public string NivelExperiencia { get; set; }

        // Construtor vazio (necessário para o Entity Framework)
        public Programador()
            : base()   // chama o construtor vazio de Utilizador
        {
            Tipo = TipoUtilizador.Programador;
        }

        // Construtor personalizado
        public Programador(
            string nome,
            string username,
            string password,
            string nivelExperiencia,
            int gestorId
        ) : base(nome, username, password, TipoUtilizador.Programador)
        {
            NivelExperiencia = nivelExperiencia;
            GestorId = gestorId;
        }

        // ToString para apresentar de forma útil na UI e no debug
        public override string ToString()
        {
            // Exemplo: "João Martins (Programador – Sénior)"
            return $"{Nome} (Programador – {NivelExperiencia})";
        }
    }
}

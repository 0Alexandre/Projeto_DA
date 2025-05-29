using iTasks;
using System.Linq;

namespace iTasks.Controllers
{
    // Controlador responsável pela lógica de autenticação do utilizador
    public class LoginController
    {
        // Instância do contexto da base de dados
        private readonly AppDbContext _context;

        // Construtor que inicializa o contexto ao criar o controlador
        public LoginController()
        {
            _context = new AppDbContext();
        }

        // Método que tenta autenticar o utilizador com base no username e password
        public Utilizador Entrar(string username, string password)
        {
            // Procura um utilizador com o username fornecido
            var utilizador = _context.Utilizadores
                .FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

            // Verifica se encontrou o utilizador e se a password corresponde
            if (utilizador != null && utilizador.Password == password)
            {
                // Login bem-sucedido: retorna o utilizador autenticado
                return utilizador;
            }

            // Caso contrário, retorna null (erro no login)
            return null;
        }
    }
}


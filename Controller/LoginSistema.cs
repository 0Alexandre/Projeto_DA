using iTasks;
using System.Linq;

namespace iTasks.Controllers
{
    // Controlador responsável pela lógica de autenticação e criação de utilizadores
    public class LoginController
    {
        // Mantém o contexto de BD para toda a duração do controlador
        private readonly AppDbContext _context;

        // Construtor: inicializa o contexto de base de dados
        public LoginController()
        {
            _context = new AppDbContext();
        }

        // Tenta autenticar pelo username e password fornecidos
        public Utilizador Entrar(string username, string password)
        {
            // Procura o utilizador pelo username (ignora maiúsculas/minúsculas)
            var utilizador = _context.Utilizadores
                .FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

            // Se existir e a password corresponder, retorna-o; senão, null
            if (utilizador != null && utilizador.Password == password)
                return utilizador;

            return null;
        }

        // Verifica se há pelo menos um utilizador na base de dados
        public bool ExisteUtilizadores()
        {
            return _context.Utilizadores.Any();
        }

        // Cria um novo utilizador (ex: no primeiro arranque ou via form)
        public bool CriarUtilizador(Utilizador u)
        {
            // Garante username único (case-insensitive)
            if (_context.Utilizadores.Any(x => x.Username.ToLower() == u.Username.ToLower()))
                return false;              // falha se já existe

            // Adiciona e grava no BD
            _context.Utilizadores.Add(u);
            _context.SaveChanges();
            return true;                   // sucesso
        }
    }
}
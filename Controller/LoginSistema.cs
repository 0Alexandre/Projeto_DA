using iTasks;
using System.Linq;

namespace iTasks.Controllers
{
    public class LoginController
    {
        private readonly AppDbContext _context;

        public LoginController()
        {
            _context = new AppDbContext();
        }

        public bool Entrar(string username, string password)
        {
            var utilizador = _context.Utilizadores
                .FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

            return utilizador != null && utilizador.Password == password;
        }
    }
}

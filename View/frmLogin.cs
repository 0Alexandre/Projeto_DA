using iTasks.Controllers;
using iTasks.View;
using System;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent(); // Inicializa os componentes do form (botões, textboxes, etc.)
        }

        // Método que executa o processo de login
        private void login()
        {
            // Lê e limpa espaços em branco dos campos
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Controlador que contém a lógica de autenticação
            LoginController controller = new LoginController();

            // Tenta autenticar com os dados fornecidos
            Utilizador utilizadorEncontrado = controller.Entrar(username, password);

            if (utilizadorEncontrado != null)
            {
                // Armazena o utilizador autenticado na sessão global
                Sessao.UtilizadorGuardado = utilizadorEncontrado;

                MessageBox.Show("Login efetuado com sucesso!");

                // Abre o form principal e esconde o de login
                frmKanban kanban = new frmKanban();
                kanban.Show();
                this.Hide();
            }
            else
            {
                // Aviso caso nome de utilizador ou senha estejam incorretos
                MessageBox.Show("Credenciais incorretas.");
            }
        }

        // Evento disparado quando o form de login é exibido
        private void frmLogin_Load(object sender, EventArgs e)
        {
            var loginCtrl = new LoginController();

            // Se a BD ainda não tem utilizadores, abre o form para criar o primeiro Gestor
            if (!loginCtrl.ExisteUtilizadores())
            {
                using (var frm = new frmCriarConta(isPrimeiro: true))
                {
                    // Se cancelar a criação, encerra a aplicação
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                }
            }
        }

        // Eventos vazios (ligados pelo Designer) — mantêm a compatibilidade
        private void txtUsername_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }

        // Clique no botão “Login”
        private void btLogin_Click(object sender, EventArgs e)
        {
            login(); // Chama o método de autenticação
        }

        // Clique no link “Criar Conta” (modo manual, para além do gestor inicial)
        private void criarConta_Click(object sender, EventArgs e)
        {
            // Abre o form de criação de conta em modo normal
            frmCriarConta frmCriarConta = new frmCriarConta();
            frmCriarConta.ShowDialog();
        }
    }
}
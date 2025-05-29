using iTasks.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace iTasks
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            label_Personalizado();
        }

        private void login()
        {
            // Guarda os dados introduzidos pelo utilizador no formulário (sem espaços em branco)
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Cria uma instância do controlador responsável pelo login
            LoginController controller = new LoginController();

            // Tenta autenticar o utilizador com os dados introduzidos
            Utilizador utilizadorEncontrado = controller.Entrar(username, password);

            // Se o utilizador foi encontrado com sucesso
            if (utilizadorEncontrado != null)
            {
                // Guarda o utilizador na sessão para uso noutras janelas
                Sessao.UtilizadorGuardado = utilizadorEncontrado;

                // Mostra uma mensagem de sucesso
                MessageBox.Show("Login efetuado com sucesso!");

                // Abre o formulario principal (Kanban) e esconde o formulario atual (formLogin)
                frmKanban kanban = new frmKanban();
                kanban.Show();
                this.Hide();
            }
            else
            {
                // Se as credenciais estiverem incorretas, mostra uma mensagem de erro
                MessageBox.Show("Credenciais incorretas.");
            }
        }


        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void criarConta_Click(object sender, EventArgs e)
        {

        }

        private void label_Personalizado()
        {
            criarConta.MouseEnter += (sender, e) =>
            {
                criarConta.ForeColor = System.Drawing.Color.Blue;
                criarConta.Font = new System.Drawing.Font(criarConta.Font, System.Drawing.FontStyle.Underline);
            };

            criarConta.MouseLeave += (sender, e) =>
            {
                criarConta.ForeColor = System.Drawing.Color.Blue;
                criarConta.Font = new System.Drawing.Font(criarConta.Font, System.Drawing.FontStyle.Regular); // Remove o underline
            };
        }
    }
}

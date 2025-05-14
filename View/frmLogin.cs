using iTasks.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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
        }

        private void login()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            LoginController controller = new LoginController();

            if (controller.Entrar(username, password))
            {
                MessageBox.Show("Login efetuado com sucesso!");
            }
            else
            {
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
    }
}

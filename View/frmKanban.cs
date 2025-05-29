using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmKanban : Form
    {
        public frmKanban()
        {
            InitializeComponent();

            // Mostra o nome do utilizador que se autenticou
            label1.Text = $"Bem-vindo -> {Sessao.UtilizadorGuardado.Nome}!";
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Fecha toda a aplicação
            System.Windows.Forms.Application.Exit();
        }
    }
}

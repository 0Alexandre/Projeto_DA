using iTasks.Controller;
using iTasks.Model;
using System;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmGereTiposTarefas : Form
    {
        private readonly TipoTarefaControlador ctrl = new TipoTarefaControlador();

        public frmGereTiposTarefas()
        {
            InitializeComponent();
            // liga o evento Load ao nosso método
            this.Load += frmGereTiposTarefas_Load;
        }

        // Carrega a lista de tipos quando o form abre
        private void frmGereTiposTarefas_Load(object sender, EventArgs e)
        {
            AtualizarLista();
        }

        // Preenche o ListBox e limpa campos
        private void AtualizarLista()
        {
            lstLista.DataSource = ctrl.ListarTipos();
            lstLista.DisplayMember = "Nome";
            lstLista.ClearSelected();

            txtId.Clear();
            txtDesc.Clear();
        }

        // Quando o utilizador escolhe um item, mostra nos TextBoxes
        private void lstLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLista.SelectedItem is TipoTarefa t)
            {
                txtId.Text = t.Id.ToString();
                txtDesc.Text = t.Nome;
            }
            else
            {
                txtId.Clear();
                txtDesc.Clear();
            }
        }

        // Cria ou atualiza o tipo de tarefa
        private void btGravar_Click(object sender, EventArgs e)
        {
            var nome = txtDesc.Text.Trim();
            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Preencha o nome do tipo.");
                return;
            }

            // Se tiver Id vazio, criamos; caso contrário, atualizamos
            if (string.IsNullOrEmpty(txtId.Text))
            {
                if (!ctrl.CriarTipo(new TipoTarefa { Nome = nome }))
                    MessageBox.Show("Já existe um tipo com esse nome.");
            }
            else
            {
                int id = int.Parse(txtId.Text);
                if (!ctrl.AtualizarTipo(new TipoTarefa { Id = id, Nome = nome }))
                    MessageBox.Show("Erro ao atualizar o tipo.");
            }

            AtualizarLista();
        }

        // Stubs vazios do Designer (manter para ligar eventos)
        private void txtId_TextChanged(object sender, EventArgs e) { }
        private void txtDesc_TextChanged(object sender, EventArgs e) { }
    }
}
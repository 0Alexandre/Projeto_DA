using iTasks.Controller;
using iTasks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmGereUtilizadores : Form
    {
        private readonly GestorControlador gestorCtrl = new GestorControlador();
        private readonly ProgramadorControlador progCtrl = new ProgramadorControlador();

        public frmGereUtilizadores()
        {
            InitializeComponent();
        }

        private void frmGereUtilizadores_Load(object sender, EventArgs e)
        {
            AtualizarListas();
            PreencherCombos();
            lstListaGestores.ClearSelected();
            lstListaProgramadores.ClearSelected();
        }

        private void AtualizarListas()
        {
            lstListaGestores.DataSource = gestorCtrl.ListarGestores();
            lstListaGestores.DisplayMember = "Nome";
            lstListaProgramadores.DataSource = progCtrl.ListarProgramadores();
            lstListaProgramadores.DisplayMember = "Nome";
        }

        private void PreencherCombos()
        {
            // Departamentos hard-coded
            cbDepartamento.DataSource = new List<string>
            {
                "TI",
                "Financeiro",
                "RH",
                "Outro"
            };

            // Níveis de experiência hard-coded
            cbNivelProg.DataSource = new List<string>
            {
                "Júnior",
                "Pleno",
                "Sênior"
            };

            // Lista de Gestores ainda vem da BD
            var gestores = gestorCtrl.ListarGestores();
            cbGestorProg.DataSource = gestores;
            cbGestorProg.DisplayMember = "Nome";
            cbGestorProg.ValueMember = "Id";
        }

        private void lstListaGestores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstListaGestores.SelectedItem is Gestor g)
            {
                txtIdGestor.Text = g.Id.ToString();
                txtNomeGestor.Text = g.Nome;
                txtUsernameGestor.Text = g.Username;
                txtPasswordGestor.Text = g.Password;
                cbDepartamento.SelectedItem = g.Departamento;
                chkGereUtilizadores.Checked = g.GereUtilizadores;
            }
            else
            {
                txtIdGestor.Clear();
                txtNomeGestor.Clear();
                txtUsernameGestor.Clear();
                txtPasswordGestor.Clear();
                cbDepartamento.SelectedIndex = -1;
                chkGereUtilizadores.Checked = false;
            }
        }

        private void btGravarGestor_Click(object sender, EventArgs e)
        {
            var gestor = new Gestor
            {
                Nome = txtNomeGestor.Text.Trim(),
                Username = txtUsernameGestor.Text.Trim(),
                Password = txtPasswordGestor.Text.Trim(),
                Departamento = cbDepartamento.SelectedItem.ToString(),
                GereUtilizadores = chkGereUtilizadores.Checked,
                Tipo = TipoUtilizador.Gestor
            };

            if (gestorCtrl.CriarGestor(gestor))
                AtualizarListas();
        }

        private void lstListaProgramadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstListaProgramadores.SelectedItem is Programador p)
            {
                txtIdProg.Text = p.Id.ToString();
                txtNomeProg.Text = p.Nome;
                txtUsernameProg.Text = p.Username;
                txtPasswordProg.Text = p.Password;
                cbNivelProg.SelectedItem = p.NivelExperiencia;
                cbGestorProg.SelectedValue = p.GestorId;
            }
            else
            {
                txtIdProg.Clear();
                txtNomeProg.Clear();
                txtUsernameProg.Clear();
                txtPasswordProg.Clear();
                cbNivelProg.SelectedIndex = -1;
                cbGestorProg.SelectedIndex = -1;
            }
        }

        private void btGravarProg_Click(object sender, EventArgs e)
        {
            var prog = new Programador
            {
                Nome = txtNomeProg.Text.Trim(),
                Username = txtUsernameProg.Text.Trim(),
                Password = txtPasswordProg.Text.Trim(),
                NivelExperiencia = cbNivelProg.SelectedItem.ToString(),
                GestorId = (int)cbGestorProg.SelectedValue,
                Tipo = TipoUtilizador.Programador
            };

            if (progCtrl.CriarProgramador(prog))
                AtualizarListas();
        }

        private void btnRemoverGestor_Click(object sender, EventArgs e)
        {
            if (lstListaGestores.SelectedItem is Gestor g)
            {
                var resp = MessageBox.Show(
                    $"Remover o gestor “{g.Nome}”?",
                    "Confirmação",
                    MessageBoxButtons.YesNo);

                if (resp == DialogResult.Yes && gestorCtrl.RemoverGestor(g.Id))
                    AtualizarListas();
                else
                    MessageBox.Show("Não foi possível remover o gestor.");
            }
        }

        private void btnRemoverProg_Click(object sender, EventArgs e)
        {
            if (lstListaProgramadores.SelectedItem is Programador p)
            {
                var resp = MessageBox.Show(
                    $"Remover o programador “{p.Nome}”?",
                    "Confirmação",
                    MessageBoxButtons.YesNo);

                if (resp == DialogResult.Yes && progCtrl.RemoverProgramador(p.Id))
                    AtualizarListas();
                else
                    MessageBox.Show("Não foi possível remover o programador.");
            }
        }

        private void btLimparSelecao1_Click(object sender, EventArgs e)
        {
            lstListaGestores.ClearSelected();
        }

        private void btLimparSelecao2_Click(object sender, EventArgs e)
        {
            lstListaProgramadores.ClearSelected();
        }

        // --- stubs gerados pelo Designer, mantidos vazios ---
        private void txtIdProg_TextChanged(object sender, EventArgs e) { }
        private void txtNomeProg_TextChanged(object sender, EventArgs e) { }
        private void txtUsernameProg_TextChanged(object sender, EventArgs e) { }
        private void txtPasswordProg_TextChanged(object sender, EventArgs e) { }
        private void cbNivelProg_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cbGestorProg_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtIdGestor_TextChanged(object sender, EventArgs e) { }
        private void txtNomeGestor_TextChanged(object sender, EventArgs e) { }
        private void txtUsernameGestor_TextChanged(object sender, EventArgs e) { }
        private void txtPasswordGestor_TextChanged(object sender, EventArgs e) { }
        private void cbDepartamento_SelectedIndexChanged(object sender, EventArgs e) { }
        private void chkGereUtilizadores_CheckedChanged(object sender, EventArgs e) { }
    }
}

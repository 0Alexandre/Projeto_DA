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
        // Controladores para Gestores e Programadores
        private readonly GestorControlador gestorCtrl = new GestorControlador();
        private readonly ProgramadorControlador progCtrl = new ProgramadorControlador();

        public frmGereUtilizadores()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Quando o formulário carrega, atualiza as listas e os combos,
        /// e limpa qualquer seleção prévia.
        /// </summary>
        private void frmGereUtilizadores_Load(object sender, EventArgs e)
        {
            AtualizarListas();
            PreencherCombos();
            lstListaGestores.ClearSelected();        // remove seleção inicial
            lstListaProgramadores.ClearSelected();   // remove seleção inicial
        }

        /// <summary>
        /// Carrega do banco de dados todos os Gestores e Programadores,
        /// e define como DataSource dos respectivos ListBoxes.
        /// </summary>
        private void AtualizarListas()
        {
            // Lista de gestores (mostra apenas o Nome)
            lstListaGestores.DataSource = gestorCtrl.ListarGestores();
            lstListaGestores.DisplayMember = "Nome";

            // Lista de programadores (mostra apenas o Nome)
            lstListaProgramadores.DataSource = progCtrl.ListarProgramadores();
            lstListaProgramadores.DisplayMember = "Nome";
        }

        /// <summary>
        /// Preenche os ComboBoxes com valores:
        /// • Departamentos fixos
        /// • Níveis de experiência fixos
        /// • Lista de gestores para associação de programador
        /// </summary>
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

            // Para atribuir Gestor a Programador, lista Gestores atuais
            var gestores = gestorCtrl.ListarGestores();
            cbGestorProg.DataSource = gestores;
            cbGestorProg.DisplayMember = "Nome";
            cbGestorProg.ValueMember = "Id";
        }

        /// <summary>
        /// Ao selecionar um Gestor no ListBox, preenche os campos de edição.
        /// Se nada estiver selecionado, limpa os campos.
        /// </summary>
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
                // Limpa tudo se nenhum gestor selecionado
                txtIdGestor.Clear();
                txtNomeGestor.Clear();
                txtUsernameGestor.Clear();
                txtPasswordGestor.Clear();
                cbDepartamento.SelectedIndex = -1;
                chkGereUtilizadores.Checked = false;
            }
        }

        /// <summary>
        /// Botão GRAVAR para Gestor: cria novo ou atualiza existente.
        /// Após sucesso, recarrega a lista de gestores.
        /// </summary>
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

            // Se criar/atualizar sem erros, atualiza a lista
            if (gestorCtrl.CriarGestor(gestor))
                AtualizarListas();
        }

        /// <summary>
        /// Ao selecionar um Programador no ListBox, preenche seus campos.
        /// Se nada selecionado, limpa os campos.
        /// </summary>
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
                // Limpa campos se nenhum programador selecionado
                txtIdProg.Clear();
                txtNomeProg.Clear();
                txtUsernameProg.Clear();
                txtPasswordProg.Clear();
                cbNivelProg.SelectedIndex = -1;
                cbGestorProg.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Botão GRAVAR para Programador: cria novo ou atualiza existente.
        /// Após sucesso, recarrega a lista de programadores.
        /// </summary>
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

            // Se criar/atualizar sem erros, atualiza a lista
            if (progCtrl.CriarProgramador(prog))
                AtualizarListas();
        }

        /// <summary>
        /// Botão REMOVER Gestor: pergunta confirmação, e remove via controlador.
        /// </summary>
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

        /// <summary>
        /// Botão REMOVER Programador: pergunta confirmação, e remove via controlador.
        /// </summary>
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

        /// <summary>
        /// Limpa a seleção do ListBox de gestores.
        /// </summary>
        private void btLimparSelecao1_Click(object sender, EventArgs e)
        {
            lstListaGestores.ClearSelected();
        }

        /// <summary>
        /// Limpa a seleção do ListBox de programadores.
        /// </summary>
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
        private void groupBox3_Enter(object sender, EventArgs e) { }
    }
}
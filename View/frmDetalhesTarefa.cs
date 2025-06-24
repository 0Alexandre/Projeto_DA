using iTasks.Controller;
using iTasks.Model;
using System;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmDetalhesTarefa : Form
    {
        private readonly TarefaControlador tarefaControlador = new TarefaControlador();
        private readonly Tarefa tarefaAtual;
        private readonly bool modoVisualizacao;

        public frmDetalhesTarefa(Tarefa tarefa, bool modoVisualizacao)
        {
            InitializeComponent();
            this.tarefaAtual = tarefa;
            this.modoVisualizacao = modoVisualizacao;
        }

        // Mantido: trata de inicializar o formulário
        private void frmDetalhesTarefa_Load(object sender, EventArgs e)
        {
            PreencherCombos();

            if (tarefaAtual != null)
                PreencherCampos(tarefaAtual);

            // Campos que nunca se editam
            txtId.ReadOnly =
            txtEstado.ReadOnly =
            txtDataCriacao.ReadOnly =
            txtDataRealini.ReadOnly =
            txtdataRealFim.ReadOnly = true;

            // Só permite edição se for criação (tarefaAtual==null) ou gestor
            bool podeEditar = tarefaAtual == null
                              || Sessao.UtilizadorGuardado.Tipo == TipoUtilizador.Gestor;

            txtDesc.ReadOnly = !podeEditar;
            cbTipoTarefa.Enabled = podeEditar;
            cbProgramador.Enabled = podeEditar;
            txtOrdem.ReadOnly = !podeEditar;
            txtStoryPoints.ReadOnly = !podeEditar;
            dtInicio.Enabled = podeEditar;
            dtFim.Enabled = podeEditar;
        }

        private void PreencherCombos()
        {
            cbTipoTarefa.DataSource = tarefaControlador.ListarTiposTarefa();
            cbTipoTarefa.DisplayMember = "Nome";
            cbProgramador.DataSource = tarefaControlador.ListarProgramadoresDoGestor(Sessao.UtilizadorGuardado.Id);
            cbProgramador.DisplayMember = "Nome";
        }

        private void PreencherCampos(Tarefa t)
        {
            txtId.Text = t.Id.ToString();
            txtEstado.Text = t.EstadoAtual;
            txtDataCriacao.Text = t.DataCriacao.ToString("yyyy-MM-dd");
            txtDesc.Text = t.Descricao;
            cbTipoTarefa.SelectedValue = t.IdTipoTarefa;
            cbProgramador.SelectedValue = t.IdProgramador;
            txtOrdem.Text = t.OrdemExecucao.ToString();
            txtStoryPoints.Text = t.StoryPoints.ToString();
            dtInicio.Value = t.DataPrevistaInicio;
            dtFim.Value = t.DataPrevistaFim;
            txtDataRealini.Text = t.DataRealInicio?.ToString("yyyy-MM-dd") ?? "";
            txtdataRealFim.Text = t.DataRealFim?.ToString("yyyy-MM-dd") ?? "";
        }

        private void btGravar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tarefaAtual == null)
                {
                    var nova = new Tarefa(
                        txtDesc.Text.Trim(),
                        "ToDo",
                        int.Parse(txtStoryPoints.Text.Trim())
                    )
                    {
                        IdTipoTarefa = (int)cbTipoTarefa.SelectedValue,
                        IdProgramador = (int)cbProgramador.SelectedValue,
                        IdGestor = Sessao.UtilizadorGuardado.Id,
                        OrdemExecucao = int.Parse(txtOrdem.Text.Trim()),
                        DataPrevistaInicio = dtInicio.Value,
                        DataPrevistaFim = dtFim.Value
                    };

                    if (tarefaControlador.CriarTarefa(nova))
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Ordem já existe para este programador.");
                    }
                }
                else
                {
                    // Reutiliza tarefaAtual para não perder Id/data criação
                    tarefaAtual.Descricao = txtDesc.Text.Trim();
                    tarefaAtual.IdTipoTarefa = (int)cbTipoTarefa.SelectedValue;
                    tarefaAtual.IdProgramador = (int)cbProgramador.SelectedValue;
                    tarefaAtual.OrdemExecucao = int.Parse(txtOrdem.Text.Trim());
                    tarefaAtual.StoryPoints = int.Parse(txtStoryPoints.Text.Trim());
                    tarefaAtual.DataPrevistaInicio = dtInicio.Value;
                    tarefaAtual.DataPrevistaFim = dtFim.Value;

                    if (tarefaControlador.AtualizarTarefa(tarefaAtual))
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Ordem já existe para este programador.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void btFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Stubs vazios usados pelo Designer, não removidos
        private void txtEstado_TextChanged(object sender, EventArgs e) { }
        private void cbTipoTarefa_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtDesc_TextChanged(object sender, EventArgs e) { }
        private void cbProgramador_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtOrdem_TextChanged(object sender, EventArgs e) { }
        private void txtStoryPoints_TextChanged(object sender, EventArgs e) { }
        private void dtInicio_ValueChanged(object sender, EventArgs e) { }
        private void dtFim_ValueChanged(object sender, EventArgs e) { }
        private void txtDataRealini_TextChanged(object sender, EventArgs e) { }
        private void txtdataRealFim_TextChanged(object sender, EventArgs e) { }
        private void txtId_TextChanged(object sender, EventArgs e){}
    }
}

using iTasks.Controller;
using iTasks.Model;
using System;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmDetalhesTarefa : Form
    {
        // Controlador para operações de CRUD em Tarefa
        private readonly TarefaControlador tarefaControlador = new TarefaControlador();

        // Tarefa atual (null = vamos criar nova)
        private readonly Tarefa tarefaAtual;

        // True = modo só leitura; false = modo edição/criação
        private readonly bool modoVisualizacao;

        // Construtor: recebe, opcionalmente, uma tarefa e o modo de visualização
        public frmDetalhesTarefa(Tarefa tarefa, bool modoVisualizacao)
        {
            InitializeComponent();
            this.tarefaAtual = tarefa;
            this.modoVisualizacao = modoVisualizacao;
        }

        // Evento disparado ao carregar o form
        private void frmDetalhesTarefa_Load(object sender, EventArgs e)
        {
            PreencherCombos();  // Popular dropdowns de tipos e programadores

            if (tarefaAtual != null)
            {
                // Se for edição, preencher os campos com os valores existentes
                PreencherCampos(tarefaAtual);
            }
            else
            {
                // Se for criação, definir valores iniciais
                txtEstado.Text = "ToDo";
                txtDataCriacao.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDataRealini.Clear();
                txtdataRealFim.Clear();
            }

            // Campos que nunca podem ser editados manualmente
            txtId.ReadOnly =
            txtEstado.ReadOnly =
            txtDataCriacao.ReadOnly =
            txtDataRealini.ReadOnly =
            txtdataRealFim.ReadOnly = true;

            // Só permitimos edição se estivermos a criar (tarefaAtual==null)
            // ou se for um Gestor no modo visualização
            bool podeEditar = tarefaAtual == null || Sessao.UtilizadorGuardado.Tipo == TipoUtilizador.Gestor;

            // Ativar/desativar edição nos campos conforme a variável podeEditar
            txtDesc.ReadOnly = !podeEditar;
            cbTipoTarefa.Enabled = podeEditar;
            cbProgramador.Enabled = podeEditar;
            txtOrdem.ReadOnly = !podeEditar;
            txtStoryPoints.ReadOnly = !podeEditar;
            dtInicio.Enabled = podeEditar;
            dtFim.Enabled = podeEditar;
        }

        // Preenche os comboboxes com dados do controlador
        private void PreencherCombos()
        {
            // Tipos de tarefa
            cbTipoTarefa.DataSource = tarefaControlador.ListarTiposTarefa();
            cbTipoTarefa.DisplayMember = "Nome"; // Propriedade para mostrar
            cbTipoTarefa.ValueMember = "Id";   // Valor associado

            // Programadores vinculados a este gestor
            cbProgramador.DataSource = tarefaControlador.ListarProgramadoresDoGestor(Sessao.UtilizadorGuardado.Id);
            cbProgramador.DisplayMember = "Nome";
            cbProgramador.ValueMember = "Id";
        }

        // Carrega os campos do form com os valores de uma tarefa existente
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

        // Clique no botão "Gravar": criação ou atualização
        private void btGravar_Click(object sender, EventArgs e)
        {
            // 1) Validar o campo Ordem (deve ser inteiro ≥1)
            if (!int.TryParse(txtOrdem.Text.Trim(), out int ordem) || ordem < 1)
            {
                MessageBox.Show(
                    "Por favor, introduza um número inteiro positivo no campo Ordem.",
                    "Ordem inválida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // 2) Validar StoryPoints (inteiro ≥0)
            if (!int.TryParse(txtStoryPoints.Text.Trim(), out int storyPoints) || storyPoints < 0)
            {
                MessageBox.Show(
                    "Por favor, introduza um número inteiro não-negativo no campo Story Points.",
                    "Story Points inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                if (tarefaAtual == null)
                {
                    // Criação de nova tarefa usando construtor personalizado
                    var nova = new Tarefa(
                            txtDesc.Text.Trim(),
                            "ToDo",
                            storyPoints
                        )
                    {
                        IdTipoTarefa = (int)cbTipoTarefa.SelectedValue,
                        IdProgramador = (int)cbProgramador.SelectedValue,
                        IdGestor = Sessao.UtilizadorGuardado.Id,
                        OrdemExecucao = ordem,
                        DataPrevistaInicio = dtInicio.Value,
                        DataPrevistaFim = dtFim.Value
                    };

                    // Tenta criar e fecha se tiver sucesso
                    if (tarefaControlador.CriarTarefa(nova))
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        // Ordem duplicada → alerta
                        MessageBox.Show(
                            "Já existe uma tarefa com essa ordem para este programador.",
                            "Erro de ordem duplicada",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
                else
                {
                    // Atualizar campos na tarefa existente
                    tarefaAtual.Descricao = txtDesc.Text.Trim();
                    tarefaAtual.IdTipoTarefa = (int)cbTipoTarefa.SelectedValue;
                    tarefaAtual.IdProgramador = (int)cbProgramador.SelectedValue;
                    tarefaAtual.OrdemExecucao = ordem;
                    tarefaAtual.StoryPoints = storyPoints;
                    tarefaAtual.DataPrevistaInicio = dtInicio.Value;
                    tarefaAtual.DataPrevistaFim = dtFim.Value;

                    // Tenta atualizar e fecha se tiver sucesso
                    if (tarefaControlador.AtualizarTarefa(tarefaAtual))
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        // Ordem duplicada → alerta
                        MessageBox.Show(
                            "Já existe uma tarefa com essa ordem para este programador.",
                            "Erro de ordem duplicada",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção inesperada e mostra ao utilizador
                MessageBox.Show(
                    $"Ocorreu um erro inesperado ao gravar a tarefa:\n{ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Fecha o form sem gravar
        private void btFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Stubs vazios gerados pelo Designer — não removê-los
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
        private void txtId_TextChanged(object sender, EventArgs e) { }
    }
}
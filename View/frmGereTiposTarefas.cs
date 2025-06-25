using iTasks.Controller;
using iTasks.Model;
using System;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmGereTiposTarefas : Form
    {
        // Controlador responsável pelo CRUD de Tipos de Tarefa
        private readonly TipoTarefaControlador ctrl = new TipoTarefaControlador();

        public frmGereTiposTarefas()
        {
            InitializeComponent();

            // Liga o evento Load deste Form ao nosso método customizado
            this.Load += frmGereTiposTarefas_Load;
        }

        /// <summary>
        /// Executado quando o formulário é carregado em memória.
        /// Chama AtualizarLista para preencher o ListBox.
        /// </summary>
        private void frmGereTiposTarefas_Load(object sender, EventArgs e)
        {
            AtualizarLista();
        }

        /// <summary>
        /// Carrega todos os tipos de tarefa e limpa os campos de edição.
        /// </summary>
        private void AtualizarLista()
        {
            // Busca todos os tipos do controlador e define como fonte de dados
            lstLista.DataSource = ctrl.ListarTipos();
            // Exibe a propriedade Nome de cada TipoTarefa
            lstLista.DisplayMember = "Nome";
            // Garante que nenhum item esteja pré-selecionado
            lstLista.ClearSelected();

            // Limpa os TextBoxes de Id e Descrição
            txtId.Clear();
            txtDesc.Clear();
        }

        /// <summary>
        /// Disparado quando o utilizador muda a seleção no ListBox.
        /// Se um TipoTarefa for selecionado, preenche os TextBoxes correspondentes.
        /// </summary>
        private void lstLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLista.SelectedItem is TipoTarefa t)
            {
                // Mostra o Id e o Nome do tipo selecionado
                txtId.Text = t.Id.ToString();
                txtDesc.Text = t.Nome;
            }
            else
            {
                // Se nada estiver selecionado, limpa os campos
                txtId.Clear();
                txtDesc.Clear();
            }
        }

        /// <summary>
        /// Botão Gravar: cria novo ou atualiza tipo existente conforme o campo txtId.
        /// </summary>
        private void btGravar_Click(object sender, EventArgs e)
        {
            // Lê e limpa o texto do nome
            var nome = txtDesc.Text.Trim();
            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Preencha o nome do tipo.");
                return;
            }

            // Se txtId estiver vazio, trata-se de criação
            if (string.IsNullOrEmpty(txtId.Text))
            {
                // Tenta criar. Se falhar (nome duplicado), mostra erro.
                if (!ctrl.CriarTipo(new TipoTarefa { Nome = nome }))
                    MessageBox.Show("Já existe um tipo com esse nome.");
            }
            else
            {
                // Caso contrário, parseia o Id e tenta atualizar
                int id = int.Parse(txtId.Text);
                if (!ctrl.AtualizarTipo(new TipoTarefa { Id = id, Nome = nome }))
                    MessageBox.Show("Erro ao atualizar o tipo.");
            }

            // Depois de criar ou atualizar, recarrega a lista
            AtualizarLista();
        }

        private void btEliminar_Click(object sender, EventArgs e)
        {
            // Só prossegue se houver um Id válido selecionado
            if (int.TryParse(txtId.Text, out int id))
            {
                var resultado = MessageBox.Show(
                    "Tem a certeza que quer eliminar este tipo de tarefa?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (resultado == DialogResult.Yes)
                {
                    // Remove via controlador
                    if (ctrl.RemoverTipo(id))
                    {
                        MessageBox.Show("Tipo eliminado com sucesso.");
                        AtualizarLista();
                    }
                    else
                    {
                        MessageBox.Show("Tipo não encontrado.");
                    }
                }
            }
        }

        private void btLimparSelecao_Click(object sender, EventArgs e)
        {
            lstLista.ClearSelected();
        }

        // Stubs (event handlers vazios) gerados pelo Designer — não remover,
        // pois o Designer referencia esses métodos para ligar eventos.
        private void txtId_TextChanged(object sender, EventArgs e) { }
        private void txtDesc_TextChanged(object sender, EventArgs e) { }
    }
}
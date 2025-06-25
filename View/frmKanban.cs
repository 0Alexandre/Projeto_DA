using iTasks.Controller;
using iTasks.Model;
using System;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmKanban : Form
    {
        // Controlador para manipular as operações sobre Tarefa
        private readonly TarefaControlador tarefaCtrl = new TarefaControlador();

        public frmKanban()
        {
            InitializeComponent();
            // Exibe o nome do utilizador logado na etiqueta
            label1.Text = $"Bem-vindo -> {Sessao.UtilizadorGuardado.Nome}!";
        }

        // Evento disparado quando o formulário é carregado
        private void frmKanban_Load(object sender, EventArgs e)
        {
            AtualizarListas();

            // Define quais botões/menu estarão habilitados conforme o tipo de utilizador
            var tipo = Sessao.UtilizadorGuardado.Tipo;
            gerirUtilizadoresToolStripMenuItem.Enabled = (tipo == TipoUtilizador.Gestor);
            gerirTiposDeTarefasToolStripMenuItem.Enabled = (tipo == TipoUtilizador.Gestor);
            btNova.Enabled = (tipo == TipoUtilizador.Gestor);
            btSetDoing.Enabled = btSetTodo.Enabled = btSetDone.Enabled = (tipo != TipoUtilizador.Gestor);
            btRemover.Enabled = (tipo == TipoUtilizador.Gestor);
            exportarParaCSVToolStripMenuItem.Enabled = (tipo == TipoUtilizador.Gestor);
        }

        // Atualiza as ListBox ToDo, Doing e Done com base no utilizador
        private void AtualizarListas()
        {
            var user = Sessao.UtilizadorGuardado;

            // Se for Gestor, lista as tarefas criadas por ele; senão, apenas as atribuídas ao Programador
            lstTodo.DataSource = (user.Tipo == TipoUtilizador.Gestor)
                ? tarefaCtrl.ListarTarefasPorGestor(user.Id, "ToDo")
                : tarefaCtrl.ListarTarefasPorProgramador(user.Id, "ToDo");
            lstDoing.DataSource = (user.Tipo == TipoUtilizador.Gestor)
                ? tarefaCtrl.ListarTarefasPorGestor(user.Id, "Doing")
                : tarefaCtrl.ListarTarefasPorProgramador(user.Id, "Doing");
            lstDone.DataSource = (user.Tipo == TipoUtilizador.Gestor)
                ? tarefaCtrl.ListarTarefasPorGestor(user.Id, "Done")
                : tarefaCtrl.ListarTarefasPorProgramador(user.Id, "Done");

            // Limpa qualquer seleção anterior
            lstTodo.ClearSelected();
            lstDoing.ClearSelected();
            lstDone.ClearSelected();
        }

        // Botão "Nova Tarefa" abre o formulário de detalhe em modo criação
        private void btNova_Click(object sender, EventArgs e)
        {
            var frm = new frmDetalhesTarefa(null, false);
            if (frm.ShowDialog() == DialogResult.OK)
                AtualizarListas();  // Recarrega listas se nova tarefa foi criada
            frm.Dispose();         // Libera recursos do form criado
        }

        // Duplo clique em ToDo → abre o detalhe em modo só leitura
        private void lstTodo_DoubleClick(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa t)
            {
                var frm = new frmDetalhesTarefa(t, true);
                frm.ShowDialog();
                AtualizarListas();  // Atualiza após fechar
                frm.Dispose();
            }
        }

        // Botão "Executar" (ToDo→Doing): só para programador, verifica limite de 2 tarefas em Doing
        private void btSetDoing_Click(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa t &&
                tarefaCtrl.PodeExecutarTarefa(t.IdProgramador))
            {
                tarefaCtrl.MudarEstado(t.Id, "Doing");
                AtualizarListas();
            }
        }

        // Botão "Reiniciar" (Doing→ToDo): permite voltar se não estiver Done
        private void btSetTodo_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa t && t.EstadoAtual != "Done")
            {
                tarefaCtrl.MudarEstado(t.Id, "ToDo");
                AtualizarListas();
            }
        }

        // Botão "Terminar" (Doing→Done): só se for a tarefa de menor ordem
        private void btSetDone_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa t &&
                tarefaCtrl.PodeTerminarTarefa(t.IdProgramador, t.OrdemExecucao))
            {
                tarefaCtrl.MudarEstado(t.Id, "Done");
                AtualizarListas();
            }
        }

        // Botão "Remover" elimina tarefa em ToDo, apenas se for do Gestor logado
        private void btRemover_Click(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa t &&
                tarefaCtrl.RemoverTarefa(t.Id, Sessao.UtilizadorGuardado.Id))
            {
                AtualizarListas();
            }
        }

        // Menu "Exportar para CSV" para tarefas concluídas (só gestor)
        private void exportarParaCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "tarefas_concluidas.csv"
            };
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            bool sucesso = tarefaCtrl.ExportarConcluidasParaCsv(
                Sessao.UtilizadorGuardado.Id,
                dlg.FileName
            );

            // Mostra resultado da exportação
            MessageBox.Show(
                sucesso ? "Exportação concluída!" : "Erro na exportação.",
                "Exportar CSV",
                MessageBoxButtons.OK,
                sucesso ? MessageBoxIcon.Information : MessageBoxIcon.Error
            );
        }

        // Stubs de eventos sem lógica adicional (ligados pelo Designer)
        private void label1_Click(object sender, EventArgs e) { }
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
            => Application.Exit();
        private void gerirUtilizadoresToolStripMenuItem_Click(object sender, EventArgs e)
            => new frmGereUtilizadores().Show();
        private void gerirTiposDeTarefasToolStripMenuItem_Click(object sender, EventArgs e)
            => new frmGereTiposTarefas().Show();
        private void tarefasTerminadasToolStripMenuItem_Click(object sender, EventArgs e)
            => new frmConsultarTarefasConcluidas().Show();
        private void tarefasEmCursoToolStripMenuItem_Click(object sender, EventArgs e)
            => new frmConsultaTarefasEmCurso().Show();
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void lstDoing_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lstDone_SelectedIndexChanged(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void lstTodo_SelectedIndexChanged(object sender, EventArgs e) { }

        // Botão "Previsão" (implementação futura)
        private void btPrevisao_Click(object sender, EventArgs e)
        {
        }

        // Botão para limpar seleção em todas as listas
        private void btLimparSelecao_Click(object sender, EventArgs e)
        {
            lstTodo.ClearSelected();
            lstDoing.ClearSelected();
            lstDone.ClearSelected();
        }
    }
}
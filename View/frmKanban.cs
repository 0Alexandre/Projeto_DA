using iTasks.Controller;
using iTasks.Model;
using System;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmKanban : Form
    {
        private readonly TarefaControlador tarefaCtrl = new TarefaControlador();

        public frmKanban()
        {
            InitializeComponent();
            label1.Text = $"Bem-vindo -> {Sessao.UtilizadorGuardado.Nome}!";
        }

        private void frmKanban_Load(object sender, EventArgs e)
        {
            AtualizarListas();
            var tipo = Sessao.UtilizadorGuardado.Tipo;

            gerirUtilizadoresToolStripMenuItem.Enabled = (tipo == TipoUtilizador.Gestor);
            gerirTiposDeTarefasToolStripMenuItem.Enabled = (tipo == TipoUtilizador.Gestor);
            btNova.Enabled = (tipo == TipoUtilizador.Gestor);
            btSetDoing.Enabled = btSetTodo.Enabled = btSetDone.Enabled = (tipo != TipoUtilizador.Gestor);
            btRemover.Enabled = (tipo == TipoUtilizador.Gestor);
            exportarParaCSVToolStripMenuItem.Enabled = (tipo == TipoUtilizador.Gestor);
        }

        private void AtualizarListas()
        {
            var user = Sessao.UtilizadorGuardado;
            lstTodo.DataSource = (user.Tipo == TipoUtilizador.Gestor)
                ? tarefaCtrl.ListarTarefasPorGestor(user.Id, "ToDo")
                : tarefaCtrl.ListarTarefasPorProgramador(user.Id, "ToDo");
            lstDoing.DataSource = (user.Tipo == TipoUtilizador.Gestor)
                ? tarefaCtrl.ListarTarefasPorGestor(user.Id, "Doing")
                : tarefaCtrl.ListarTarefasPorProgramador(user.Id, "Doing");
            lstDone.DataSource = (user.Tipo == TipoUtilizador.Gestor)
                ? tarefaCtrl.ListarTarefasPorGestor(user.Id, "Done")
                : tarefaCtrl.ListarTarefasPorProgramador(user.Id, "Done");

            lstTodo.ClearSelected();
            lstDoing.ClearSelected();
            lstDone.ClearSelected();
        }

        private void btNova_Click(object sender, EventArgs e)
        {
            var frm = new frmDetalhesTarefa(null, false);
            if (frm.ShowDialog() == DialogResult.OK)
                AtualizarListas();
            frm.Dispose();
        }

        private void lstTodo_DoubleClick(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa t)
            {
                var frm = new frmDetalhesTarefa(t, true);
                frm.ShowDialog();
                AtualizarListas();
                frm.Dispose();
            }
        }

        private void btSetDoing_Click(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa t &&
                tarefaCtrl.PodeExecutarTarefa(t.IdProgramador))
            {
                tarefaCtrl.MudarEstado(t.Id, "Doing");
                AtualizarListas();
            }
        }

        private void btSetTodo_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa t && t.EstadoAtual != "Done")
            {
                tarefaCtrl.MudarEstado(t.Id, "ToDo");
                AtualizarListas();
            }
        }

        private void btSetDone_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa t &&
                tarefaCtrl.PodeTerminarTarefa(t.IdProgramador, t.OrdemExecucao))
            {
                tarefaCtrl.MudarEstado(t.Id, "Done");
                AtualizarListas();
            }
        }

        private void btRemover_Click(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa t &&
                tarefaCtrl.RemoverTarefa(t.Id, Sessao.UtilizadorGuardado.Id))
            {
                AtualizarListas();
            }
        }

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

            string msg = sucesso ? "Exportação concluída!" : "Erro na exportação.";
            var icon = sucesso ? MessageBoxIcon.Information : MessageBoxIcon.Error;
            MessageBox.Show(msg, "Exportar CSV", MessageBoxButtons.OK, icon);
        }

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
        private void button1_Click(object sender, EventArgs e){}
        private void lstTodo_SelectedIndexChanged(object sender, EventArgs e){}

        private void btPrevisao_Click(object sender, EventArgs e)
        {
        }

        private void btLimparSelecao_Click(object sender, EventArgs e)
        {
            lstTodo.ClearSelected();
            lstDoing.ClearSelected();
            lstDone.ClearSelected();
        }
    }
}
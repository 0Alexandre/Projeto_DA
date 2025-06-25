using iTasks.Controllers;
using System;
using System.Windows.Forms;

namespace iTasks.View
{
    public partial class frmCriarConta : Form
    {
        // Indica se este é o primeiro utilizador a ser criado (sempre Gestor)
        private bool _isPrimeiro;

        public frmCriarConta(bool isPrimeiro = false)
        {
            InitializeComponent();    // Inicializa componentes do form (TextBoxes, Buttons, etc.)
            _isPrimeiro = isPrimeiro; // Guarda o modo (primeiro gestor ou criação normal)
        }

        // Evento disparado ao carregar o botão "Criar Conta"
        private void btCriarConta_Click(object sender, EventArgs e)
        {
            // Monta um novo utilizador com dados dos campos
            var novo = new Utilizador
            {
                Nome = txtNome.Text.Trim(),     // Nome completo
                Username = txtUsername.Text.Trim(), // Identificador de login
                Password = txtPassword.Text.Trim(), // Palavra-passe
                Tipo = TipoUtilizador.Gestor,   // Primeiro sempre Gestor; ajustar se não for
                GestorId = null                     // Sem gestor associado no registo inicial
            };

            var ctrl = new LoginController();         // Controlador de login/criação
            bool ok = ctrl.CriarUtilizador(novo);     // Tenta gravar na base de dados

            if (!ok)
            {
                // Se falhar (username duplicado), avisa e mantém o form aberto
                MessageBox.Show("Username já existe.");
                return;
            }

            // Se correu bem, fecha o form indicando sucesso
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Stubs criados pelo Designer — necessários para ligar eventos, mas sem lógica
        private void label2_Click(object sender, EventArgs e) { }
        private void txtNome_TextChanged(object sender, EventArgs e) { }
        private void frmCriarConta_Load(object sender, EventArgs e) { }
    }
}
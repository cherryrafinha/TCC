using MySql.Data.MySqlClient;
using System.Windows;

namespace TCC
{

    public partial class CriarUsuario : Window
    {
        public CriarUsuario()
        {
            InitializeComponent();
        }

        private void GoToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            PaginaLogin loginWindow = new PaginaLogin();
            loginWindow.Show();
            this.Close();
        }

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNomeCadastro.Text.Trim();
            string email = txtEmailCadastro.Text.Trim();
            string senha = txtSenhaCadastro.Password;

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    // Verifica se o e-mail já está cadastrado
                    string verificaEmailQuery = "SELECT COUNT(*) FROM Usuario WHERE Email = @Email";
                    using (var cmdVerifica = new MySqlCommand(verificaEmailQuery, conexao))
                    {
                        cmdVerifica.Parameters.AddWithValue("@Email", email);
                        long count = (long)cmdVerifica.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Este e-mail já está em uso.");
                            return;
                        }
                    }

                    // Insere o novo usuário
                    string inserirQuery = "INSERT INTO Usuario (Nome, Email, Senha) VALUES (@Nome, @Email, @Senha)";
                    using (var cmd = new MySqlCommand(inserirQuery, conexao))
                    {
                        cmd.Parameters.AddWithValue("@Nome", nome);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Senha", senha); // ⚠️ Em produção, use hash

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuário criado com sucesso!");

                    // Redireciona para tela de login
                    PaginaLogin loginWindow = new PaginaLogin();
                    loginWindow.Show();
                    this.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro ao criar usuário: " + ex.Message);
            }
        }
    }
}
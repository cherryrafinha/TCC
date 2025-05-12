using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace TCC
{

    public partial class PaginaLogin : Window
    {
        public PaginaLogin()
        {
            InitializeComponent();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string senha = txtSenha.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    string query = "SELECT * FROM Usuario WHERE Email = @Email AND Senha = @Senha";

                    using (var cmd = new MySqlCommand(query, conexao))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Senha", senha);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MessageBox.Show("Login bem-sucedido!");
                                // Abrir próxima tela, ex:
                                // new TelaPrincipal().Show(); this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Credenciais inválidas.", "Erro de Login", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar com o banco de dados:\n" + ex.Message);
            }
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            CriarUsuario createUserWindow = new CriarUsuario();
            createUserWindow.Show();
            this.Hide(); // Oculta a tela de login
        }
    }
}

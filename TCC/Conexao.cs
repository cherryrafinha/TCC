using MySql.Data.MySqlClient;

namespace TCC
{

    public static class Conexao
    {
        private static string connectionString = "server=localhost;database=TCCBase;uid=root;pwd=milu15;";

        public static MySqlConnection ObterConexao()
        {
            var conexao = new MySqlConnection(connectionString);
            conexao.Open();
            return conexao;
        }
    }

}

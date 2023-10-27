using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Sistema_de_Gerenciamento
{
    class Conexao
    {
        //Faz a conexão do banco de dados
        public string conect = "SERVER=localhost; PORT=; DATABASE=; UID= ; PWD=;";
        public NpgsqlConnection con = null;

        //metodo
        public void AbrirConexao()
        {

            con = new NpgsqlConnection(conect);
            con.Open();
        }

        public void FecharConexao()
        {
            con = new NpgsqlConnection(conect);
            con.Close();
            con.Dispose();//Derruba Conexões abertas
            con.CloseAsync(); //Limpar
        }
    }
}

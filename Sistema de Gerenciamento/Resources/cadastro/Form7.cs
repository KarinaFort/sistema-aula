using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Sistema_de_Gerenciamento.Resources.cadastro
{
    public partial class pesquisaDeFuncionarios : Form
    {
        Conexao con = new Conexao();
        string sql;
        public pesquisaDeFuncionarios()
        {
            InitializeComponent();
        }

        private void pesquisaDeFuncionarios_Load(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            Limpar();
        }
        public void Limpar()
        {
            name.Text = "";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string nome = name.Text;
            Listar(nome);
            FormatarGrid();
        }

        public void Listar(string nome)
        {
            con.AbrirConexao();
            sql = "SELECT * FROM funcionarios WHERE nome ILIKE @nome";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con.con))
            {
                cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");
                NpgsqlDataAdapter da = new NpgsqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                grid.DataSource = dt;
                con.FecharConexao();
            }

        }
        private void FormatarGrid()
        {

            grid.Columns[1].HeaderText = "Data de nascimento";
            grid.Columns[2].HeaderText = "CPF";
            grid.Columns[3].HeaderText = "Celular";
            grid.Columns[4].HeaderText = "Endereço";
            grid.Columns[5].HeaderText = "Foto";
            grid.Columns[0].HeaderText = "Nome";


            grid.Columns[0].Width = 110;
            grid.Columns[6].Width = 110;
            grid.Columns[0].Visible = true;
            grid.Columns[7].Visible = true;

        }

    }
}

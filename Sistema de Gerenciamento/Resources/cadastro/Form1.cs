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
    public partial class pesquisaDeProdutos : Form
    {
        Conexao con = new Conexao();
        string sql;
        public pesquisaDeProdutos()
        {
            InitializeComponent();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void pesquisaDeProdutos_Load(object sender, EventArgs e)
        {

        }
        

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string nome = name.Text;
            Listar(nome);
            FormatarGrid();

        }

        //MÉTODOS
        public void Listar(string nome)
        {
            con.AbrirConexao();
            sql = "SELECT * FROM produtos WHERE nome ILIKE @nome";
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

            grid.Columns[1].HeaderText = "1";
            grid.Columns[2].HeaderText = "1";
            grid.Columns[3].HeaderText = "1";
            grid.Columns[4].HeaderText = "1";
            grid.Columns[5].HeaderText = "1";
            grid.Columns[0].HeaderText = "1";


            grid.Columns[0].Width = 110;
            grid.Columns[6].Width = 110;
            grid.Columns[0].Visible = true;
            grid.Columns[6].Visible = true;

        }

        public void Limpar()
        {
            name.Text = "";
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            Limpar();

        }
    }
}


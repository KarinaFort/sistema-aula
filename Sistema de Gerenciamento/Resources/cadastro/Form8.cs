using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Sistema_de_Gerenciamento.Resources.cadastro
{
    public partial class pesquisaDeClientes : Form
    {

        Conexao con = new Conexao();
        string sql;
        
        public pesquisaDeClientes()
        {
            InitializeComponent();
            
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            name.Enabled = true;
            Limpar();
            name.Focus();

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pesquisaDeClientes_Load(object sender, EventArgs e)
        {
            DesabilitarBtn();
            name.Enabled = false;

        }



        private void btnExcluir_Click(object sender, EventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

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
            sql = "SELECT * FROM cliente WHERE nome ILIKE @nome";
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
        public void FormatarGrid()
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

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }
        public void DesabilitarBtn()
        {
           
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
            btnCancelar.Enabled = false;

        }
        public void HabilitarBtn()
        {
           
            btnNovo.Enabled = false;
            btnCancelar.Enabled = true;
            btnSalvar.Enabled = true;
            btnEditar.Enabled = true;
            btnExcluir.Enabled = true;
        }

        public void Limpar()
        {
            name.Text = "";
        }

        private void name_TextChanged(object sender, EventArgs e)
        {

        }

        private void grid_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            
            HabilitarBtn();
            btnNovo.Enabled = false;
            con.AbrirConexao();

            {
                if (e.RowIndex > -1)
                {

                    name.Text = grid.CurrentRow.Cells[0].Value.ToString();//Manda para a caixa de texto o nome correto a ser excluído
                    if (grid.CurrentRow.Cells[5].Value != DBNull.Value)//Verifica informação do banco de dados
                    {
                        byte[] imagem = (byte[])grid.Rows[e.RowIndex].Cells[5].Value;//variavel byte[] criada p/ receber o que vem da grid em bytes
                        MemoryStream ms = new MemoryStream(imagem);//recebe a variavel ja com o valor da grid convertido 

                        fotoPerfil.Image = Image.FromStream(ms);
                    }
                    else
                    {
                        fotoPerfil.Image = Properties.Resources.add_48px;//coloca a imagem que indica sem foto
                    }
                }
                else
                {
                    return;
                }
                con.FecharConexao();

            }


        }

        private void btnExcluir_Click_1(object sender, EventArgs e)
        {
            try
            {
                var resposta = MessageBox.Show("Você realmente deseja excluir este cadastro? ", "Deletar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resposta == DialogResult.Yes)
                {
                    con.AbrirConexao();
                    sql = "DELETE FROM cliente WHERE nome = @nome";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con.con))
                    {
                        cmd.Parameters.AddWithValue("@nome", name.Text.ToUpper());
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // O registro foi excluído com sucesso
                            MessageBox.Show("Registro excluído com sucesso!! ", "Deletar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Chame o método Listar com o nome para atualizar a grid
                            Listar(name.Text.ToUpper());

                            name.Enabled = false;
                            DesabilitarBtn();
                        }
                        else
                        {
                            // Nenhum registro foi excluído
                            MessageBox.Show("Nenhum registro encontrado para exclusão.", "Deletar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    con.FecharConexao();
                }
            }
            catch (Exception ex)
            {
                // Trate qualquer exceção que possa ocorrer
                MessageBox.Show("Ocorreu um erro ao excluir o registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }
        
    }

}

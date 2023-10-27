using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Npgsql;


namespace Sistema_de_Gerenciamento
{
    public partial class cadastroDeProdutos : Form
    {
        Conexao con = new Conexao();


        string sql;

        public cadastroDeProdutos()
        {
            InitializeComponent();
        }

        private void cadastroDeProdutos_Load(object sender, EventArgs e)
        {
            DesabilitarBtn();
            DesabilitarCampos();

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            HabilitarBtn();

            LimparCampos();
            nomeProduto.Focus();
            btnNovo.Enabled = false;
            nomeProduto.Enabled = true;
            registro.Enabled = true;
            quantidade.Enabled = true;
            fornecedor.Enabled = true;

        }

        public void DesabilitarBtn()
        {
            btnSalvar.Enabled = false;
            btnNovo.Enabled = true;
            btnExcluir.Enabled = false;
            btnCancelar.Enabled = false;
            btnEditar.Enabled = false;

        }
        public void HabilitarBtn()
        {
            btnSalvar.Enabled = true;
            nomeProduto.Enabled = true;
            btnNovo.Enabled = false;
            btnCancelar.Enabled = true;
            btnExcluir.Enabled = true;
            btnEditar.Enabled = true;
        }

        public void LimparCampos()
        {
            nomeProduto.Text = "";
            registro.Text = "";
            quantidade.Text = "";
            fornecedor.Text = "";
            codigo.Text = "";
            preco.Text = "";
        }

        public void Habilitarcampos()
        {
            nomeProduto.Enabled = true;
            registro.Enabled = false;
            quantidade.Enabled = true;
            fornecedor.Enabled = true;
        }
        public void DesabilitarCampos()
        {
            nomeProduto.Enabled = false;
            registro.Enabled = false;
            quantidade.Enabled = false;
            fornecedor.Enabled = false;

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (nomeProduto.Text.Trim() == "")
            {
                MessageBox.Show("Preencha o campo nome", "Cadastro de produtos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nomeProduto.Text = "";
                nomeProduto.Focus();
                return;
            }

            // Inicializa uma nova conexão com o banco de dados


            con.AbrirConexao();

            sql = "INSERT INTO produtos(nome, codigo, fornecedor, quantidade, preco) " +
                         "VALUES(@nome, @codigo, @fornecedor, @quantidade, @preco)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con.con))

            {
                cmd.Parameters.AddWithValue("@nome", nomeProduto.Text.ToUpper());
                cmd.Parameters.AddWithValue("@codigo", codigo.Text);
                cmd.Parameters.AddWithValue("@fornecedor", fornecedor.Text);
                cmd.Parameters.AddWithValue("@id", registro.Text);
                cmd.Parameters.AddWithValue("@preco", Convert.ToDecimal(preco.Text));
                int quantidadeInt;
                if (int.TryParse(quantidade.Text, out quantidadeInt))
                {
                    cmd.Parameters.AddWithValue("@quantidade", quantidadeInt);
                }
                else
                {
                    // Tratar o erro se o valor não for um número inteiro válido
                    // Você pode mostrar uma mensagem de erro ou tomar outra ação apropriada aqui.
                }
                

                cmd.ExecuteNonQuery();
            }


            LimparCampos();
            DesabilitarCampos();
            DesabilitarBtn();
            
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (nomeProduto.Text.Trim() == "")
            {
                MessageBox.Show("Preencha o campo nome", "Cadastro de produtos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nomeProduto.Text = "";
                nomeProduto.Focus();
                return;
            }

            // Inicializa uma nova conexão com o banco de dados


            con.AbrirConexao();

            sql = "UPDATE produtos SET nome = @nome, codigo = @codigo, fornecedor = @fornecedor, quantidade = @quantidade, preco = @preco WHERE codigo = @codigo";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con.con))

            {
                cmd.Parameters.AddWithValue("@nome", nomeProduto.Text.ToUpper());
                cmd.Parameters.AddWithValue("@codigo", codigo.Text);
                cmd.Parameters.AddWithValue("@fornecedor", fornecedor.Text);
                int quantidadeInt;
                if (int.TryParse(quantidade.Text, out quantidadeInt))
                {
                    cmd.Parameters.AddWithValue("@quantidade", quantidadeInt);
                }
                else
                {
                    // Tratar o erro se o valor não for um número inteiro válido
                    // Você pode mostrar uma mensagem de erro ou tomar outra ação apropriada aqui.
                }
                cmd.Parameters.AddWithValue("@id", registro.Text);
                cmd.Parameters.AddWithValue("@preco", Convert.ToDecimal(preco.Text));
                cmd.ExecuteNonQuery();
            }


            LimparCampos();
            DesabilitarCampos();
            DesabilitarBtn();

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {

            try
            {
                var resposta = MessageBox.Show("Você realmente deseja excluir este cadastro? ", "Deletar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resposta == DialogResult.Yes)
                {
                    con.AbrirConexao();
                    sql = "DELETE FROM produtos WHERE nome = @nome";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con.con))
                    {
                        cmd.Parameters.AddWithValue("@nome", nomeProduto.Text.ToUpper());
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // O registro foi excluído com sucesso
                            MessageBox.Show("Registro excluído com sucesso!! ", "Deletar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Chame o método Listar com o nome para atualizar a grid
                            Listar(nomeProduto.Text.ToUpper());

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
                    name.Enabled = true;
                    name.Text = "";
                }
            }
            catch (Exception ex)
            {
                // Trate qualquer exceção que possa ocorrer
                MessageBox.Show("Ocorreu um erro ao excluir o registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            LimparCampos();
            DesabilitarCampos();
            DesabilitarBtn();

        }

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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            HabilitarBtn();
            Habilitarcampos();
            btnNovo.Enabled = false;
            btnSalvar.Enabled = false;
            btnEditar.Enabled = true;
            con.AbrirConexao();

            {
                if (e.RowIndex > -1)
                {

                    nomeProduto.Text = grid.CurrentRow.Cells[1].Value.ToString();//Manda para a caixa de texto o nome correto a ser excluído
                    registro.Text = grid.CurrentRow.Cells[0].Value.ToString();
                    quantidade.Text = grid.CurrentRow.Cells[5].Value.ToString();
                    fornecedor.Text = grid.CurrentRow.Cells[3].Value.ToString();
                    codigo.Text = grid.CurrentRow.Cells[2].Value.ToString();
                    preco.Text = grid.CurrentRow.Cells[6].Value.ToString();
                }
                else
                {
                    return;
                }
                con.FecharConexao();

            }
        }
        public void FormatarGrid()
        {

            grid.Columns[1].HeaderText = "Nome";
            grid.Columns[2].HeaderText = "Código";
            grid.Columns[3].HeaderText = "fornecedor";
            grid.Columns[5].HeaderText = "Quantidade";
            grid.Columns[0].HeaderText = "Registro";
            grid.Columns[6].HeaderText = "Preço";


            grid.Columns[0].Width = 110;
            
            grid.Columns[0].Visible = true;

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string nome = name.Text;
            Listar(nome);
            FormatarGrid();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

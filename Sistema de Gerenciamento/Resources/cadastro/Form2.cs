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
    public partial class cadastroDeClientes : Form
    {
        Conexao con = new Conexao();
        
        
        string sql;
        string foto;


        
        public cadastroDeClientes()
        {
            InitializeComponent();
        }
        private void cadastroDeClientes_Load(object sender, EventArgs e)
        {
            LimparFoto();
        }
        private void btnNovo_Click(object sender, EventArgs e)
        {
            HabilitarBtn();
            LimparCampos();
            nome.Focus();
            btnNovo.Enabled = false;
            LimparFoto();
            
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (nome.Text.Trim() == "")
            {
                MessageBox.Show("Preencha o campo nome", "Cadastro de clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nome.Text = "";
                nome.Focus();
                return;
            }

            if (cpf.Text.Length < 11)
            {
                MessageBox.Show("Preencha o campo CPF", "Cadastro de clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cpf.Focus();
                return;
            }

            // Inicializa uma nova conexão com o banco de dados
            
            
                con.AbrirConexao();

                sql = "INSERT INTO cliente(nome, dtnasc, cpf, contato, endereco, foto, data, hora ) " +
                             "VALUES(@nome, @dtnasc, @cpf, @contato, @endereco, @foto, CURRENT_DATE, CURRENT_TIME)";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con.con))

                {
                    cmd.Parameters.AddWithValue("@nome", nome.Text.ToUpper());
                    if (DateTime.TryParse(dt.Text, out DateTime dtNascValue))
                    {
                        cmd.Parameters.AddWithValue("@dtnasc", dtNascValue);
                    }
                    else
                    {
                        MessageBox.Show("Preencha a data de nascimento corretamente, separado por 00-00-0000",
                            "Cadastro de clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dt.Focus();
                        return;
                    }

                    
                    
                    cmd.Parameters.AddWithValue("@contato", contato.Text);
                    cmd.Parameters.AddWithValue("@endereco", end.Text);
                    cmd.Parameters.AddWithValue("@foto", img()); //metodo para tratar imagem no BD

                // VERIFICAR SE O CPF JA EXISTE NO SISTEMA
                // Primeiro, construa uma consulta SQL para verificar a existência do CPF
                string consultaCPFExistente = "SELECT COUNT(*) FROM cliente WHERE cpf = @cpf";

                using (NpgsqlCommand cmdVerificarCPF = new NpgsqlCommand(consultaCPFExistente, con.con))
                {
                    cmdVerificarCPF.Parameters.AddWithValue("@cpf", cpf.Text); // Substitua "cpf.Text" pelo valor do CPF a ser verificado

                    int count = Convert.ToInt32(cmdVerificarCPF.ExecuteScalar());

                    if (count > 0)
                    {
                        // O CPF já existe no sistema, você pode mostrar uma mensagem de erro aqui
                        MessageBox.Show("CPF já cadastrado no sistema.");
                        cpf.Text = "";
                        cpf.Focus();
                        return;
                    }
                    else
                    {
                       
                        cmd.Parameters.AddWithValue("@cpf", cpf.Text);


                    }
                    
                }

                    cmd.ExecuteNonQuery();
                }
            

                    LimparCampos();
                    DesabilitarCampos();
                    DesabilitarBtn();
                    LimparFoto();
        }


        private void btnExcluir_Click(object sender, EventArgs e)
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
                        cmd.Parameters.AddWithValue("@nome", nome.Text.ToUpper());
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // O registro foi excluído com sucesso
                            MessageBox.Show("Registro excluído com sucesso!! ", "Deletar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Chame o método Listar com o nome para atualizar a grid
                            Listar(nome.Text.ToUpper());

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

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            DesabilitarCampos();
            DesabilitarBtn();
            LimparCampos();
            Close();
        }
        //Métodos 
        public void DesabilitarBtn()
        {
            btnSalvar.Enabled = false;
            btnNovo.Enabled = true;
            btnExcluir.Enabled = false;
            btnCancelar.Enabled = false;

        }
        public void HabilitarBtn()
        {
            btnSalvar.Enabled = true;
            nome.Enabled = true;
            cpf.Enabled = true;
            dt.Enabled = true;
            contato.Enabled = true;
            end.Enabled = true;
            fotoPerfil.Enabled = true;
            btnNovo.Enabled = false;
            btnCancelar.Enabled = true;
            btnExcluir.Enabled = true;
        }

        public void LimparCampos()
        {
            nome.Text = "";
            cpf.Text = "";
            end.Text = "";
            dt.Text = "";
            contato.Text = "";
        }
        public void DesabilitarCampos()
        {
            nome.Enabled = false;
            cpf.Enabled = false;
            contato.Enabled = false;
            end.Enabled = false;
            fotoPerfil.Enabled = false;

        }

        private void fotoPerfil_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Imagens(*.jpg; *.png) | *jpg; *png";//mostra quais tipos de arquivos são aceitos 
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                foto = dialog.FileName.ToString(); // Pega o caminho da imagem
                fotoPerfil.ImageLocation = foto; // Exibe a imagem no Form
            }
        }

        private byte[] img()//método para enviar a imagem para o banco de dados
        {
            byte[] imagem_byte = null; //Variavel que envia o comprimeto da imagem 
            if (foto == "")
            {
                return null;
            }

            FileStream fs = new FileStream(foto, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            imagem_byte = br.ReadBytes((int)fs.Length);
            return imagem_byte;

        }
        private void LimparFoto()
        {
            fotoPerfil.Image = Properties.Resources.add_48px;
            foto = "img/add_48px.png";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string nome = name.Text;
            Listar(nome);
            FormatarGrid();
        }

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
            grid.Columns[5].Visible = false;

        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            HabilitarBtn();
            btnNovo.Enabled = false;
            btnSalvar.Enabled = false;
            btnEditar.Enabled = true;
            con.AbrirConexao();

            {
                if (e.RowIndex > -1)
                {

                    nome.Text = grid.CurrentRow.Cells[0].Value.ToString();//Manda para a caixa de texto o nome correto a ser excluído
                    cpf.Text = grid.CurrentRow.Cells[2].Value.ToString();
                    contato.Text = grid.CurrentRow.Cells[3].Value.ToString();
                    end.Text = grid.CurrentRow.Cells[4].Value.ToString();
                    dt.Text = grid.CurrentRow.Cells[1].Value.ToString();
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

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (nome.Text.Trim() == "")
            {
                MessageBox.Show("Preencha o campo nome", "Cadastro de clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nome.Text = "";
                nome.Focus();
                return;
            }

            if (cpf.Text.Length < 11)
            {
                MessageBox.Show("Preencha o campo CPF", "Cadastro de clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cpf.Focus();
                return;
            }

            // Inicializa uma nova conexão com o banco de dados


            con.AbrirConexao();

            sql = "UPDATE cliente SET nome = @nome, dtnasc = @dtnasc, cpf = @cpf, contato = @contato, endereco = @endereco, foto = @foto, data = CURRENT_DATE, hora =  CURRENT_TIME  WHERE cpf = @cpf";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con.con))

            {
                cmd.Parameters.AddWithValue("@nome", nome.Text.ToUpper());
                if (DateTime.TryParse(dt.Text, out DateTime dtNascValue))
                {
                    cmd.Parameters.AddWithValue("@dtnasc", dtNascValue);
                }
                else
                {
                    MessageBox.Show("Preencha a data de nascimento corretamente, separado por 00-00-0000",
                        "Cadastro de clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dt.Focus();
                    return;
                }


                cmd.Parameters.AddWithValue("@cpf", cpf.Text);
                cmd.Parameters.AddWithValue("@contato", contato.Text);
                cmd.Parameters.AddWithValue("@endereco", end.Text);
                cmd.Parameters.AddWithValue("@foto", img()); //metodo para tratar imagem no BD
                cmd.ExecuteNonQuery();
            }

            LimparCampos();
            DesabilitarCampos();
            DesabilitarBtn();
            LimparFoto();
            Listar(nome.Text);
            btnEditar.Enabled = false;
            btnNovo.Enabled = true;
            name.Text = "";
        }
    }
}

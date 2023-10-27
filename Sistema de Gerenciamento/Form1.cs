using Sistema_de_Gerenciamento.Resources.cadastro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_de_Gerenciamento
{
    public partial class TelaInicial : Form
    {
        public TelaInicial()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            cadastroDeClientes frm = new cadastroDeClientes();
            frm.ShowDialog();
        }

        private void TelaInicial_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            cadastroDeProdutos frm = new cadastroDeProdutos();
            frm.ShowDialog();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            vendas frm = new vendas();
            frm.ShowDialog();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            cadastroDeFuncionarios frm = new cadastroDeFuncionarios();
            frm.ShowDialog();

        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            pesquisa frm = new pesquisa();
            frm.ShowDialog();

        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void label6_Click(object sender, EventArgs e)
        {
            abrirRelatorios frm = new abrirRelatorios();
            frm.ShowDialog();
        }
    }
}

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
    public partial class pesquisa : Form
    {
        public pesquisa()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            pesquisaDeClientes frm = new pesquisaDeClientes();
            frm.ShowDialog();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            pesquisaDeFuncionarios frm = new pesquisaDeFuncionarios();
            frm.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            pesquisaDeProdutos frm = new pesquisaDeProdutos();
            frm.ShowDialog();
        }
    }
}

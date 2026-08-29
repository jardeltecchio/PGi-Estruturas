using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{
    public partial class FOpcoesDeformacaoSolida : Form
    {
        public FOpcoesDeformacaoSolida()
        {
            InitializeComponent();
        }
        Gerenciador gerenciador;
        public FOpcoesDeformacaoSolida(Gerenciador ger)
        {
            InitializeComponent();
            gerenciador = ger;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FOpcoesDeformacaoSolida_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void FOpcoesDeformacaoSolida_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.CargaBarra = null;
        }
    }
}

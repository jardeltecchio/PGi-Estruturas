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
    public partial class FRotularElementos : Form
    {
        public FRotularElementos()
        {
            InitializeComponent();
        }
        Gerenciador gerenciador;
        public FRotularElementos(Gerenciador ger)
        {
            gerenciador = ger;
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FRotularElementos_Load(object sender, EventArgs e)
        {
            posicao.SelectedIndex = 0;
            this.Left = 50;
            this.Top = gerenciador.Height / 2;
        }

        private void FRotularElementos_FormClosed(object sender, FormClosedEventArgs e)
        {
      //      gerenciador.formDesenho.CancelaInsercoes();
            gerenciador.RotularElementos = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}

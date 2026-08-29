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
    public partial class FSobre : Form
    {
        public FSobre()
        {
            InitializeComponent();

        }
        Gerenciador gerenciador;
        public FSobre(Gerenciador ger)
        {
            InitializeComponent();
            this.gerenciador = ger;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            gerenciador.sobre = null;
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FSobre_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.sobre = null;
        }
    }
}

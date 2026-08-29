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
    public partial class FRenomearElementos : Form
    {
        public FRenomearElementos()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void chManterPrefixo_CheckedChanged(object sender, EventArgs e)
        {
            NovoPrefixo.Enabled = !chManterPrefixo.Checked;
        }

        private void chManterNumero_CheckedChanged(object sender, EventArgs e)
        {
            NumeroInicial.Enabled = !chManterNumero.Checked;
        
        }
    }
}

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
    public partial class FLayersEscolheCor : Form
    {
        public FLayersEscolheCor()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btCor.BackColor = colorDialog1.Color;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}

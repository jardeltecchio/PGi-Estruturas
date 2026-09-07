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
    public partial class FRelatorios : Form
    {
        public FRelatorios()
        {
            InitializeComponent();
        }

        public string TextoRelatorio
        {
            get { return edRelatorio.Text; }
            set
            {
                edRelatorio.Text = value ?? string.Empty;
                edRelatorio.Select(0, 0);
                edRelatorio.ScrollToCaret();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

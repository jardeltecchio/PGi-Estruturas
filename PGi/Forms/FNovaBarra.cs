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
    public partial class FNovaBarra : Form
    {
        FVisualizadorPortico owner;

        public FNovaBarra()
        {
            InitializeComponent();
        }

        public FNovaBarra(FVisualizadorPortico ow)
        {
            InitializeComponent();
            this.owner = ow;
        }

        private void FNovaBarra_Shown(object sender, EventArgs e)
        {
            this.Left = 10;
            this.Top = 200;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            owner.CancelaOperacoes();
            Close();
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                int max_barra = owner.Portico.nBarras;

                owner.GJ = double.Parse(RigidezGJ.Text);
                owner.EI = double.Parse(RigidezEI.Text);
                owner.NovaBarra = true;
            }
            catch (Exception)
            {

            }
        }

        private void FNovaBarra_FormClosed(object sender, FormClosedEventArgs e)
        {
            owner.fNovaBarra = null;
        }
    }
}

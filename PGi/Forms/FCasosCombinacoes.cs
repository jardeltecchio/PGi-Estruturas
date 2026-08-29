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
    public partial class FCasosCombinacoes : Form
    {
        Gerenciador ger;
        public FCasosCombinacoes()
        {
            InitializeComponent();
        }

        public FCasosCombinacoes(Gerenciador g)
        {
            ger = g;    
            InitializeComponent();
            ShowInTaskbar = false;
        }
        double xant = 0,yant, of_x;
        bool clicou;
        private void FCasosCombinacoes_MouseClick(object sender, MouseEventArgs e)
        {
            xant = e.X;
            yant = e.Y;
        }

        private void FCasosCombinacoes_MouseMove(object sender, MouseEventArgs e)
        {
            if (clicou)
            {
                this.Left += (int)(e.X - xant) ;
                //ger.xant = e.X;
            }
            if (clicou && Top <= (ger.Height - this.Height - 15))
            {
                this.Top += (int)(e.Y - yant);
                //ger.xant = e.X;
            }

        }

        private void FCasosCombinacoes_MouseDown(object sender, MouseEventArgs e)
        {
            clicou = true;
            xant = e.X;
            yant = e.Y;
        }

        private void FCasosCombinacoes_Move(object sender, EventArgs e)
        {
            ger.AtualizaDesenho();
          
        }

        private void panelGeral_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pnVisualizacaoCargas_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pnVisualizacaoCargas_MouseUp(object sender, MouseEventArgs e)
        {
;
        }
        public void AtualizaComboCarga()
        {
            cbCasoCarga.Items.Clear();
            cbCasoCarga.Items.Add("<NENHUM>");
            foreach (TCasosCarga c in ger.formDesenho.CasosCarga)
                cbCasoCarga.Items.Add(c.Nome);
            cbCasoCarga.SelectedIndex = 0;
            cbCasoCarga.Items.Add("<TODOS>");
        }

        private void FCasosCombinacoes_Load(object sender, EventArgs e)
        {
            AtualizaComboCarga();
        }

        private void FCasosCombinacoes_Shown(object sender, EventArgs e)
        {
            this.Height = 26;
            this.SendToBack();
        }

        private void FCasosCombinacoes_MouseUp(object sender, MouseEventArgs e)
        {
            clicou = false;
        }

        private void FCasosCombinacoes_Click(object sender, EventArgs e)
        {

        }
    }
}

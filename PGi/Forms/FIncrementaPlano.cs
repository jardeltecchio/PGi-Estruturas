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
    public partial class FIncrementaPlano : Form
    {
        public FIncrementaPlano()
        {
            InitializeComponent();
        }
        Gerenciador owner;
        public FIncrementaPlano(Gerenciador ow)
        {
            InitializeComponent();
            owner = ow;
        }
        public string plano;
        public bool PlanoCorte = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!PlanoCorte)
            {
                owner.edIncPlano.Text = edIncPlano.Text;
                owner.formDesenho.CriaPlanoTrabalho(plano, double.Parse(edIncPlano.Text));
            }
            else
                owner.AtivaModo3D();
            Close();
        }
        double posicaoInicial;
        private void button2_Click(object sender, EventArgs e)
        {
            owner.AtivaModo3D();
            owner.formDesenho.PlanoTrabalho.Posicao.z = posicaoInicial;
            Close();
        }

        double incr;
        private void btPisoCima_Click(object sender, EventArgs e)
        {
            incr += 1;
            edIncPlano.Text = incr.ToString("n2");

            atualizaPlano();

            owner.formDesenho.DesenhaObjetos();
            owner.formDesenho.glControl.SwapBuffers();
        }
        void atualizaPlano()
        {
            owner.formDesenho.AtualizaPlanoTrabalho(plano, incr);
     /*       if (plano == "xy")
            {
                owner.formDesenho.PlanoTrabalho.Normal.x = 0;
                owner.formDesenho.PlanoTrabalho.Normal.y = 0;
                owner.formDesenho.PlanoTrabalho.Normal.z = 1;

                owner.formDesenho.PlanoTrabalho.Posicao.z = incr * 100;
                owner.formDesenho.PlanoTrabalho.Posicao.x = 0;
                owner.formDesenho.PlanoTrabalho.Posicao.y = 0;
            }
            if (plano == "yz")
            {
                owner.formDesenho.PlanoTrabalho.Normal.x = 1;
                owner.formDesenho.PlanoTrabalho.Normal.y = 0;
                owner.formDesenho.PlanoTrabalho.Normal.z = 0;

                owner.formDesenho.PlanoTrabalho.Posicao.z = 0;
                owner.formDesenho.PlanoTrabalho.Posicao.x = incr * 100;
                owner.formDesenho.PlanoTrabalho.Posicao.y = 0;
            }
            if (plano == "xz")
            {
                owner.formDesenho.PlanoTrabalho.Normal.x = 0;
                owner.formDesenho.PlanoTrabalho.Normal.y = 1;
                owner.formDesenho.PlanoTrabalho.Normal.z = 0;
                
                owner.formDesenho.PlanoTrabalho.Posicao.z = 0;
                owner.formDesenho.PlanoTrabalho.Posicao.x = 0;
                owner.formDesenho.PlanoTrabalho.Posicao.y = incr * 100;
            }*/
        }

        private void btPisoBaixo_Click(object sender, EventArgs e)
        {
            incr -= 1;
            edIncPlano.Text = incr.ToString("n2");

            atualizaPlano();

         //   owner.formDesenho.PlanoTrabalho.Posicao.z = incr*100;
            owner.formDesenho.DesenhaObjetos();
            owner.formDesenho.glControl.SwapBuffers();
        }

        private void FIncrementaPlano_Load(object sender, EventArgs e)
        {
            if (edIncPlano.Text == "")
              edIncPlano.Text = "0";

            incr = double.Parse( edIncPlano.Text);
          // if (owner.ModoPlano)

            posicaoInicial = 0;
            owner.formDesenho.CriaPlanoTrabalho(plano, double.Parse(edIncPlano.Text));
            Left = 120;
            Top = 230;
        }

        private void FIncrementaPlano_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (PlanoCorte)
                owner.AtivaModo3D();

            owner.f_incPlano = null;
        }

        private void FIncrementaPlano_Move(object sender, EventArgs e)
        {
            owner.AtualizaDesenho();
        }

        private void rbxy_CheckedChanged(object sender, EventArgs e)
        {
           lbCoordPlano.Text = "Z (m):";
           plano = "xy";
           owner.formDesenho.CriaPlanoTrabalho(plano, double.Parse(edIncPlano.Text));
           owner.AtualizaDesenho();
        }

        private void rbyz_CheckedChanged(object sender, EventArgs e)
        {
            lbCoordPlano.Text = "X (m):";
            plano = "yz";
            owner.formDesenho.CriaPlanoTrabalho(plano, double.Parse(edIncPlano.Text));
            owner.AtualizaDesenho();
        }

        private void rbxz_CheckedChanged(object sender, EventArgs e)
        {
            lbCoordPlano.Text = "Y (m):";
            plano = "xz";
            owner.formDesenho.CriaPlanoTrabalho(plano, double.Parse(edIncPlano.Text));
            owner.AtualizaDesenho();
           
        }

        private void edIncPlano_KeyPress(object sender, KeyPressEventArgs e)
        {
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != a) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }
            base.OnKeyPress(e);
        }

        private void edIncPlano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                incr = double.Parse(edIncPlano.Text);
                atualizaPlano();

                owner.formDesenho.DesenhaObjetos();
                owner.formDesenho.glControl.SwapBuffers();
            }
        }

        private void edIncPlano_KeyUp(object sender, KeyEventArgs e)
        {
            if (edIncPlano.Text.Trim() == "")
            {
                edIncPlano.Text = "0";
               // MessageBox.Show("Valor não deve ser nulo");
            }
            else
            if ((edIncPlano.Text.Trim() != "-") && (edIncPlano.Text.Trim() != ",") && (edIncPlano.Text.Trim() != "."))
            {
                incr = System.Convert.ToDouble(edIncPlano.Text);
                atualizaPlano();

                owner.formDesenho.DesenhaObjetos();
                owner.formDesenho.glControl.SwapBuffers();
            }
        }

    }
}

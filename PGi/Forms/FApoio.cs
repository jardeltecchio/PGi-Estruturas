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
    public partial class FApoio : Form
    {
        public FApoio()
        {
            InitializeComponent();
        }
        public TDadosApoio Dados;
        Gerenciador gerenciador;
        public FApoio(Gerenciador _gerenciador, TDadosApoio _Dados)
        {
            this.gerenciador = _gerenciador;
            this.Dados = _Dados;
            InitializeComponent();
            ShowInTaskbar = false;
        }
        void Carregar()
        {
           dx.Checked =Dados.restringe_dx;
           dy.Checked =Dados.restringe_dy;
           dz.Checked =Dados.restringe_dz;
           rx.Checked =Dados.restringe_rx;
           ry.Checked =Dados.restringe_ry;
           rz.Checked =Dados.restringe_rz;
           mola_dx.Text = Dados.mola_dx.ToString("n2");
           mola_dy.Text = Dados.mola_dy.ToString("n2");
           mola_dz.Text = Dados.mola_dz.ToString("n2");
           mola_rx.Text = Dados.mola_rx.ToString("n2");
           mola_ry.Text = Dados.mola_ry.ToString("n2");
           mola_rz.Text = Dados.mola_rz.ToString("n2");
           
            if (dx.Checked && dy.Checked && dz.Checked && !rx.Checked && !ry.Checked && !rz.Checked)
               tipo = 1;
           else
           if (dx.Checked && dy.Checked && dz.Checked && rx.Checked && ry.Checked && rz.Checked)
               tipo = 2;
           else
               tipo = -1;
        }

        void Gravar()
        {
            if ((mola_rx.Text.Trim() == "") || (mola_ry.Text.Trim() == "") || (mola_rz.Text.Trim() == "")
                || (mola_dx.Text.Trim() == "") || (mola_dy.Text.Trim() == "") || (mola_dz.Text.Trim() == ""))
            {
                MessageBox.Show("O valor do coeficiente de mola não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

           /* if (!dx.Checked)
                if (System.Convert.ToDouble(mola_dx.Text) == 0)
                {
                   MessageBox.Show("O valor do coeficiente de mola está zerado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                   return;
                }*/

            if (dx.Checked && dy.Checked && dz.Checked && !rx.Checked && !ry.Checked && !rz.Checked)
                tipo = 1;
            else
            if (dx.Checked && dy.Checked && dz.Checked && rx.Checked && ry.Checked && rz.Checked)
                tipo = 2;
            else
                tipo = -1;
/*
            TDadosApoio dad;

            if (alterando)
                dad = Dados;
            else
            {
                dad = new TDadosApoio(tipo, 0);
                gerenciador.formDesenho.DadosApoio = dad.Clone() as TDadosApoio;
            }*/
            gerenciador.formDesenho.DadosApoio = new TDadosApoio(tipo,0);
            gerenciador.formDesenho.DadosApoio.Tipo = tipo;
            gerenciador.formDesenho.DadosApoio.restringe_dx = dx.Checked;
            gerenciador.formDesenho.DadosApoio.restringe_dy = dy.Checked;
            gerenciador.formDesenho.DadosApoio.restringe_dz = dz.Checked;
            gerenciador.formDesenho.DadosApoio.restringe_rx = rx.Checked;
            gerenciador.formDesenho.DadosApoio.restringe_ry = ry.Checked;
            gerenciador.formDesenho.DadosApoio.restringe_rz = rz.Checked;
            gerenciador.formDesenho.DadosApoio.mola_dx = System.Convert.ToDouble(mola_dx.Text);
            gerenciador.formDesenho.DadosApoio.mola_dy = System.Convert.ToDouble(mola_dy.Text);
            gerenciador.formDesenho.DadosApoio.mola_dz = System.Convert.ToDouble(mola_dz.Text);
            gerenciador.formDesenho.DadosApoio.mola_rx = System.Convert.ToDouble(mola_rx.Text);
            gerenciador.formDesenho.DadosApoio.mola_ry = System.Convert.ToDouble(mola_ry.Text);
            gerenciador.formDesenho.DadosApoio.mola_rz = System.Convert.ToDouble(mola_rz.Text);
            gerenciador.formDesenho.Alterou(true);

            if (!alterando)
                gerenciador.ComandoNovoApoio(InsercaoIndividual.Checked);

        }

        int tipo;

        public bool alterando = false;
        private void btRet_Click(object sender, EventArgs e)
        {
            dx.Checked = true;
            dy.Checked = true;
            dz.Checked = true;
            rx.Checked = false;
            ry.Checked = false;
            rz.Checked = false;

            rx_CheckedChanged(rx, e);
            ry_CheckedChanged(ry, e);
            rz_CheckedChanged(rz, e);
        }

        private void FApoio_Load(object sender, EventArgs e)
        {
            this.Left = 25;
            this.Top = 280;
            unmoladx.Text = "tf/m"; //gerenciador.cbUnForca.Text + "/" + gerenciador.cbUnComp.Text;
            unmoladz.Text = unmoladx.Text; 
            unmolady.Text = unmoladx.Text;

            unmolarx.Text = "tf.m/rad";// gerenciador.cbUnForca.Text + "." + gerenciador.cbUnComp.Text+"/rad";
            unmolary.Text = "tf.m/rad";
            unmolarz.Text = "tf.m/rad";

            if (alterando)
                Carregar();
            else
            {
                this.Text = "Nova restrição de apoio";
                btCirc_Click(btEngaste, null);
            }
        }

        private void dx_CheckedChanged(object sender, EventArgs e)
        {
            if (dx.Checked)
            {
                mola_dx.Text = "0";
                mola_dx.Enabled = false;
            }
            else
                mola_dx.Enabled = true;
        }

        private void dy_CheckedChanged(object sender, EventArgs e)
        {
            if (dy.Checked)
            {
                mola_dy.Text = "0";
                mola_dy.Enabled = false;
            }
            else
                mola_dy.Enabled = true;
        }

        private void dz_CheckedChanged(object sender, EventArgs e)
        {
            if (dz.Checked)
            {
                mola_dz.Text = "0";
                mola_dz.Enabled = false;
            }
            else
                mola_dz.Enabled = true;
        }

        private void rx_CheckedChanged(object sender, EventArgs e)
        {
            if (rx.Checked)
            {
                mola_rx.Text = "0";
                mola_rx.Enabled = false;
            }
            else
                mola_rx.Enabled = true;
        }

        private void ry_CheckedChanged(object sender, EventArgs e)
        {
            if (ry.Checked)
            {
                mola_ry.Text = "0";
                mola_ry.Enabled = false;
            }
            else
                mola_ry.Enabled = true;
        }

        private void rz_CheckedChanged(object sender, EventArgs e)
        {
            if (rz.Checked)
            {
                mola_rz.Text = "0";
                mola_rz.Enabled = false;
            }
            else
                mola_rz.Enabled = true;
        }

        private void btCirc_Click(object sender, EventArgs e)
        {
            dx.Checked = true;
            dy.Checked = true;
            dz.Checked = true;
            rx.Checked = true;
            ry.Checked = true;
            rz.Checked = true;
            dx_CheckedChanged(dx,e);
            dy_CheckedChanged(dy, e);
            dz_CheckedChanged(dz, e);
            rx_CheckedChanged(rx, e);
            ry_CheckedChanged(ry, e);
            rz_CheckedChanged(rz, e);
        }

        private void FApoio_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FApoio_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!alterando)
            {
                gerenciador.fApoio = null;
                gerenciador.formDesenho.CancelaInsercoes();
            }
        }
        public void Salvar()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            Gravar();
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void mola_dx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }
            base.OnKeyPress(e);
        }
    }
}

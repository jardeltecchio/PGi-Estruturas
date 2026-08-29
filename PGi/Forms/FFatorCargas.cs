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
    public partial class FFatorCargas : Form
    {
        Gerenciador gerenciador;
        public FFatorCargas()
        {
            InitializeComponent();
        }
        public FFatorCargas(Gerenciador g)
        {
            InitializeComponent();
            this.gerenciador = g;
        }

        private void FFatorCargas_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void FFatorCargas_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.FatorCarga = null;
        }

        private void FFatorCargas_Load(object sender, EventArgs e)
        {
            this.Left = gerenciador.Width / 4 - this.Width;
            this.Top = gerenciador.Height / 4;
            textBox1.Text = gerenciador.formDesenho.EscalaCargas.ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                gerenciador.formDesenho.EscalaCargas = System.Convert.ToDouble(textBox1.Text);
                TBarraGenerica bg;

                double max = 0;
                foreach (TCargaLinear o in gerenciador.formDesenho.CargasBarrasAtuais)
                    if (Math.Abs(o.Dados.valor) > max && o.Dados.valor != 0)
                        max = Math.Abs(o.Dados.valor);

                foreach (TCargaPontual o in gerenciador.formDesenho.CargasNosAtuais)
                    if (Math.Abs(o.Dados.valor) > max && o.Dados.valor != 0)
                        max = Math.Abs(o.Dados.valor);
                double fatorCarga = 0;

                if (max > 0)
                    fatorCarga = (1 / max) * gerenciador.formDesenho.EscalaCargas;

                foreach (TCargaLinear o in gerenciador.formDesenho.CargasBarrasAtuais)
                {
                    bg = gerenciador.formDesenho.Estrutura.barras.Find(b => b.IDBarra == o.idBarra && b.Dados.Tipo != 4);
                    if (bg != null)
                    {
                        double h_ = 0.001;// (bg.Dados.secao.propriedades.cz)/1000;
                        double v = 0.001;// (bg.Dados.secao.propriedades.cy)/ 1000;
                        o.CriaSetas(gerenciador.formDesenho.Unifilar, v, h_, fatorCarga);
                    }
                }

                gerenciador.formDesenho.AtualizaShaders();
                gerenciador.AtualizaDesenho();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}

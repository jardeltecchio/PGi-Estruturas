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
    public partial class FSecaoGenerica : Form
    {
        FDadosBarra dadosBarra;
        Gerenciador gerenciador;
        public FSecaoGenerica()
        {
            InitializeComponent();
        }
        public FSecaoGenerica(FDadosBarra dados, Gerenciador ow)
        {
            InitializeComponent();
            dadosBarra  = dados;
            gerenciador = ow;
            
        }
        public int id = -1;
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

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }
            base.OnKeyPress(e);
        }
        bool alterando;
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FSecaoGenerica_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }
        public TSecao secao;
        string tipo;
        public void Carrega(string tipo)
        {
            alterando = true;

            btCor.BackColor = Color.FromArgb(secao.Rgb[0], secao.Rgb[1], secao.Rgb[2]);

            NomeSecao.Text = secao.descricao;

            edA.Text = secao.area.ToString("n2");
            ix.Text = secao.inercia_flexao_z.ToString("n2");
            iy.Text = secao.inercia_flexao_y.ToString("n2");
            edE.Text = secao.e.ToString("n2");
            edG.Text = secao.g.ToString("n2");
            edJ.Text = secao.inercia_torcao.ToString("n2");
        }
        public void CriaSecao()
        {
            try
            {
                tipo = Const.SECAO_GENERICA;

                if (alterando)
                {
                    secao.descricao = NomeSecao.Text;
                    secao.tipo = tipo;
                    secao.alterou = true;
                }
                else
                {
                  //  secao = new TSecao(null, NomeSecao.Text, tipo);
                    secao.id = gerenciador.formDesenho.Secoes.Count;
                }

                secao.area = double.Parse(edA.Text);
                secao.inercia_flexao_z = double.Parse(ix.Text);
                secao.inercia_flexao_y = double.Parse(iy.Text);
                secao.g = double.Parse(edG.Text);
                secao.inercia_torcao = double.Parse(edJ.Text);
                secao.e = double.Parse(edE.Text);

                secao.Rgb[0] = (btCor.BackColor.R);
                secao.Rgb[1] = (btCor.BackColor.G);
                secao.Rgb[2] = (btCor.BackColor.B);

                dadosBarra.secaoSemRotacao = secao;
                secao.tipo = tipo;

                if (!alterando)
                {
                    gerenciador.formDesenho.Secoes.Add(secao);
                }

                dadosBarra.CarregaSecoes();

                if (!alterando)
                {
                  //  dadosBarra.lvSecoes.Items[dadosBarra.lvSecoes.Items.Count - 1].Selected = true;
                 //   dadosBarra.lvSecoes.Select();
                }
                dadosBarra.AtualizaSecoesEstrutura(secao.id);
                gerenciador.formDesenho.AtualizaBarras();

                /*
                var item1 = new ListViewItem(new[] {"", secao.id.ToString(), secao.descricao});
                dadosBarra.lvSecoes.Items.Add(item1);

                if (pnRetangulo.Visible)
                  dadosBarra.lvSecoes.Items[dadosBarra.lvSecoes.Items.Count-1].ImageIndex = 0;

                if (pnCirculo.Visible)
                    dadosBarra.lvSecoes.Items[dadosBarra.lvSecoes.Items.Count - 1].ImageIndex = 1;*/
            }

            catch (Exception ec)
            {
                MessageBox.Show(ec.Message);
            }
        }
        private void btSalvar_Click(object sender, EventArgs e)
        {
            if (NomeSecao.Text.Trim() == "")
            {
                MessageBox.Show("Nome não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                NomeSecao.Select();
                return;
            }
            if (edE.Text == "0.00" || edE.Text == "0" || edE.Text.Trim() == "")
            {
                MessageBox.Show("Valor não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                edE.Select();
                return;
            }
            if (edG.Text.Trim() == "")
            {
                MessageBox.Show("Valor não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                edG.Select();
                return;
            }
            if (edA.Text == "0.00" || edA.Text == "0" || edA.Text.Trim() == "")
            {
                MessageBox.Show("Valor não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                edA.Select();
                return;
            }
            if (ix.Text.Trim() == "")
            {
                MessageBox.Show("Valor não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ix.Select();
                return;
            }
            if (iy.Text.Trim() == "")
            {
                MessageBox.Show("Valor não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                iy.Select();
                return;
            }
            if (edJ.Text.Trim() == "")
            {
                MessageBox.Show("Valor não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                edJ.Select();
                return;
            }

            CriaSecao();

            this.Close();
        }

        private void btCor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btCor.BackColor = colorDialog1.Color;
        }

        private void FSecaoGenerica_Load(object sender, EventArgs e)
        {
            NomeSecao.Focus();

            this.Left = gerenciador.Width / 2 - this.Width;
            this.Top = gerenciador.DadosBarra.Top;
        }

        private void FSecaoGenerica_FormClosed(object sender, FormClosedEventArgs e)
        {
            dadosBarra.SecaoGenerica = null;
        }

        private void edG_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btSalvar.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (edA.Text != "")
                edA.Text = (double.Parse(edA.Text) * 10000).ToString("n2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ix.Text != "")
                ix.Text = (double.Parse(ix.Text) * 1E8).ToString("n2");
            if (iy.Text != "")
                iy.Text = (double.Parse(iy.Text) * 1E8).ToString("n2");
            if (edJ.Text != "")
                edJ.Text = (double.Parse(edJ.Text) * 1E8).ToString("n2");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (edG.Text != "")
                edE.Text = (double.Parse(edE.Text) / 1000).ToString("n2");

            if (edG.Text != "")
            edG.Text = (double.Parse(edG.Text) / 1000).ToString("n2");
        }
    }
}

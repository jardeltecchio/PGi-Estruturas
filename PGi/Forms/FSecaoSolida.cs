using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win32Interop.Enums;
using static PG.Geom;

namespace PG
{
    public partial class FSecaoSolida : Form
    {
        FDadosBarra dadosBarra;
        Gerenciador gerenciador;
        public FSecaoSolida()
        {
            InitializeComponent();
        }
        public void AtualizaMateriais()
        {
            cbMaterial.Items.Clear();
            foreach (TMateriais m in gerenciador.ConfiguracoesPGi.CfgProjeto.materiais)
              //  if (m.)
             cbMaterial.Items.Add(m.Descricao);
        }

        public FSecaoSolida(FDadosBarra dados, Gerenciador ow)
        {
            InitializeComponent();
            dadosBarra  = dados;
            gerenciador = ow;
            
        }

        void HabilitaSecao(object sender)
        {
            for (int i = 0; i < this.gbTipo.Controls.Count; i++)
            {
                if (this.gbTipo.Controls[i] is Panel)
                {
                    this.gbTipo.Controls[i].Visible = false;
                }
            }

            for (int i = 0; i < this.gbTipo.Controls.Count; i++)
            {
                if (this.gbTipo.Controls[i] is Panel)
                {
                    if (this.gbTipo.Controls[i].Tag == (sender as Button).Tag)
                    {
                        this.gbTipo.Controls[i].Visible = true;
                        this.gbTipo.Controls[i].Dock = DockStyle.Top;
                    }
                }
            }

            if ((sender as Button).Tag == "1")
                gbTipo.Text = "Retângulo";
            if ((sender as Button).Tag == "2")
                gbTipo.Text = "Círculo";
        }

        private void btRet_Click(object sender, EventArgs e)
        {
            HabilitaSecao(sender);

            if ((sender as Button).Tag == "2")
                edD_Validated(edD, e);
            if ((sender as Button).Tag == "1")
                edb1_Validated(edb1, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        CoordenadaD[] coordsPoligono;
        public TPoligono poligono;
        public TSecao secao;
        public void CriaSecaoConcreto()
        {
            try
            {
                double b1 = 0, h1 = 0;
                double d1 = 0;
                if (pnCirculo.Visible)
                {
                    tipo = Const.SECAO_SOLIDO_CIRC;
                    
                    d1 = double.Parse(edD.Text) / 100;
                    double raio = (d1 / 2);
                    double x = raio;
                    double y = raio;
                    b1 = raio * 2;
                    coordsPoligono = new CoordenadaD[41];

                    for (int j = 0; j <= 40; j++)
                    {
                        coordsPoligono[j].X = (x + (raio * Math.Cos(j * 6.2831 / 40)));
                        coordsPoligono[j].Y = (y + (raio * Math.Sin(j * 6.2831 / 40)));
                        coordsPoligono[j].pontoEmRaio = true;
                    }
                }

                if (pnRetangulo.Visible)
                {
                    tipo = Const.SECAO_SOLIDO_RET;
                    
                    b1 = double.Parse(edb1.Text)/100;
                    h1 = double.Parse(edh1.Text)/100;

                    coordsPoligono = new CoordenadaD[5];
                    coordsPoligono[0].X = 0;
                    coordsPoligono[0].Y = 0;

                    coordsPoligono[1].X = b1;
                    coordsPoligono[1].Y = 0;

                    coordsPoligono[2].X = b1;
                    coordsPoligono[2].Y = h1;

                    coordsPoligono[3].X = 0;
                    coordsPoligono[3].Y = h1;

                    coordsPoligono[4].X = 0;
                    coordsPoligono[4].Y = 0;
                }

                poligono = new TPoligono(coordsPoligono);
                poligono.CalculaPropriedades();
                double cx = poligono.centroide.X;
                double cy = poligono.centroide.Y;

                for (int i = 0; i < coordsPoligono.Count(); i++)
                {
                    poligono.coords[i].X = poligono.coords[i].X - cx;
                    poligono.coords[i].Y = poligono.coords[i].Y - cy;
                }

                poligono.CalculaPropriedades();

                if (Geom.Iguais(poligono.centroide.X, 0))
                    poligono.centroide.X = 0;
                if (Geom.Iguais(poligono.centroide.Y, 0))
                    poligono.centroide.Y = 0;

                if (alterando)
                {
                    secao.poligono = poligono;
                    secao.descricao = NomeSecao.Text;
                    secao.tipo = tipo;
                    secao.alterou = true;
                }
                else
                {
                    secao = new TSecao(poligono, NomeSecao.Text, tipo);
                    secao.id = gerenciador.formDesenho.Secoes.Count;
                }

                if (tipo == Const.SECAO_SOLIDO_RET)
                {
                    secao.b1 = b1;
                    secao.h1 = h1;
                    
                    double v1 = (b1 * b1 * b1) * h1;
                    double t2 = (1 - (Math.Pow(b1, 4) / (12 * Math.Pow(h1, 4))));
                    //matrix analysis of framed structures pg504 - inercia torção de retangulo
                    double v3 = 0.333 - ((0.21 * (b1 / h1)) * t2);
                    double it = v1 * v3;

                    secao.inercia_torcao  = it;
                    secao.inercia_flexao_z = poligono.Ixcg;
                    secao.inercia_flexao_y = poligono.Iycg;
                    secao.area  = poligono.area;
                }
                if (tipo == Const.SECAO_SOLIDO_CIRC)
                {
                    secao.d1 = d1;
                    secao.b1 = d1;
                    secao.h1 = d1;
                    secao.area = (RMath.M_PI * (Math.Pow((b1), 2))) / 4;
                    secao.inercia_torcao = (RMath.M_PI * (Math.Pow((b1), 4))) / 32;
                    secao.inercia_flexao_z = (RMath.M_PI * (Math.Pow((b1 / 2), 4))) / 4;
                    secao.inercia_flexao_y = secao.inercia_flexao_z;
                }

                secao.Rgb[0] = (btCor.BackColor.R);
                secao.Rgb[1] = (btCor.BackColor.G);
                secao.Rgb[2] = (btCor.BackColor.B);
                secao.idMaterial = cbMaterial.SelectedIndex;
                secao.material = gerenciador.ConfiguracoesPGi.CfgProjeto.materiais[cbMaterial.SelectedIndex];
                dadosBarra.secaoSemRotacao = secao;
                secao.PesoProprio = (secao.material.PesoEspecifico * (secao.b1) * (secao.h1)) / 100; //kf/f para kn/m
                 secao.tipo = tipo;
                if (!alterando)
                {
                    gerenciador.formDesenho.Secoes.Add(secao);
                    dadosBarra.secaoSemRotacao = secao;

                }

                dadosBarra.CarregaSecoes();

                if (!alterando)
                {

                   // dadosBarra.lvSecoes.Select();
                    dadosBarra.secaoSemRotacao = secao;
                  //  dadosBarra.lvSecoes.Items[dadosBarra.lvSecoes.Items.Count - 1].Selected = true;
                }
                dadosBarra.AtualizaSecoesEstrutura(secao.id);
                gerenciador.formDesenho.AtualizaBarras();
                gerenciador.formDesenho.Alterou(true);

                dadosBarra.RodarSecao();
                dadosBarra.DesenhaSecao();
                /*
                var item1 = new ListViewItem(new[] {"", secao.id.ToString(), secao.descricao});
                dadosBarra.lvSecoes.Items.Add(item1);

                if (pnRetangulo.Visible)
                  dadosBarra.lvSecoes.Items[dadosBarra.lvSecoes.Items.Count-1].ImageIndex = 0;

                if (pnCirculo.Visible)
                    dadosBarra.lvSecoes.Items[dadosBarra.lvSecoes.Items.Count - 1].ImageIndex = 1;*/
            }
        
            catch(Exception ec)
            {
                MessageBox.Show(ec.Message);
            }
        }
        public bool alterando;
        public void Carrega(string tipo)
        {
            alterando = true;

            if (tipo == Const.SECAO_SOLIDO_RET)
            {
                edb1.Text = (secao.b1*100).ToString("n2");
                edh1.Text = (secao.h1 * 100).ToString("n2");
                HabilitaSecao(btRet);
            }
            if (tipo == Const.SECAO_SOLIDO_CIRC)
            {
                edD.Text = (secao.d1*100).ToString("n2");
                HabilitaSecao(btCirc);
            }

            btCor.BackColor = Color.FromArgb(secao.Rgb[0],secao.Rgb[1],secao.Rgb[2]);
         
            NomeSecao.Text = secao.descricao;
            cbMaterial.SelectedIndex = secao.idMaterial;
        }
        public int id = -1;
        private void FSecaoSolida_FormClosed(object sender, FormClosedEventArgs e)
        {
            dadosBarra.SecaoSolida = null;
          //  dadosBarra.alterando = false;
            dadosBarra.Enquadrar();
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            if (cbMaterial.SelectedIndex == -1)
            {
                MessageBox.Show("O material não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            } 
             CriaSecaoConcreto();
            CriaSecaoConcreto();

            this.Close();
        }
        string tipo;
        private void FSecaoSolida_Load(object sender, EventArgs e)
        {
        
            if (!alterando)
            {
                HabilitaSecao(btRet);
                edb1_Validated(edb1, e);
            }
            else
            {

            }

            foreach (SClasses c in gerenciador.ConfiguracoesPGi.CfgProjeto.classes)
	        {
                cbMaterial.Items.Add(c.nome);
            }
       
            AtualizaMateriais();

            cbMaterial.SelectedIndex = 0;

            edb1.Focus();
            edb1.SelectAll(); 
            
            this.Left = gerenciador.Width / 2 - this.Width;
            this.Top = gerenciador.DadosBarra.Top;
        }

        private void edb1_Validated(object sender, EventArgs e)
        {
            if (! alterando)
              NomeSecao.Text = "Ret " + edb1.Text + " x " + edh1.Text;
        }

        private void edD_Validated(object sender, EventArgs e)
        {
            if (!alterando)
              NomeSecao.Text = "Circ " + edD.Text;
        }

        private void btCorCima_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btCor.BackColor = colorDialog1.Color;
        }

        private void FSecaoSolida_MouseMove(object sender, MouseEventArgs e)
        {
   
        }

        private void FSecaoSolida_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void btCor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.Close();
        //    if (e.KeyCode == Keys.Enter)
    //            btSalvar_Click(btSalvar, null);
        }

        private void FSecaoSolida_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btSalvar_Click(btSalvar, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gerenciador.Materiais == null)
                gerenciador.Materiais = new FMateriais(gerenciador);
            gerenciador.Materiais.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void edb1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void edD_KeyPress(object sender, KeyPressEventArgs e)
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
        public FCalculaSecao calculasecao;

        private void s(object sender, EventArgs e)
        {

        }
    }
}

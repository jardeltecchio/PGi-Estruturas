using OpenTK.Graphics.OpenGL;
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

    public partial class FInfoBarraPortico : Form
    {
        public FInfoBarraPortico()
        {
            InitializeComponent();
        }
        TBarraPortico barra;
        Gerenciador ger;
        public FInfoBarraPortico(TBarraPortico bar, Gerenciador gg)
        {
            ger = gg;
            this.barra = bar;
            InitializeComponent();
            edCopias.Maximum = ger.formDesenho.Estrutura.PorticoEspacial.barras.Count()-1;
        }

        List<InfoBarraPortico> Local, Global, Rotacao, MatRotacaoTransposta, M;

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void edCopias_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < ger.formDesenho.Estrutura.PorticoEspacial.barras.Count(); i++)
            {
                ger.formDesenho.Estrutura.PorticoEspacial.barras[i].Selecionado = false;
            }
           
            ger.formDesenho.Estrutura.PorticoEspacial.barras[(int)edCopias.Value].Selecionado = true;
            ger.formDesenho.AtualizarDesenho(true, true);
            ger.AtualizaDesenho();
            barra = ger.formDesenho.Estrutura.PorticoEspacial.barras[(int)edCopias.Value];
            atualiza();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        void atualiza()
        {
            /*  if (mglobal)
              {
                  Local = new List<InfoBarraPortico>();
                  int col = 0;
                  for (int i = 1; i <= 12; i++)
                  {
                      col = 0;
                      Local.Add(new InfoBarraPortico());
                      Local[Local.Count - 1].c1 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c2 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c3 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c4 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c5 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c6 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c7 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c8 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c9 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c10 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c11 = barra.MatrizLocal[i, ++col].ToString("n2");
                      Local[Local.Count - 1].c12 = barra.MatrizLocal[i, ++col].ToString("n2");
                  }
                  gridLocal.DataSource = Local;
              }*/

            Local = new List<InfoBarraPortico>();
            int col = 0;
            for (int i = 1; i <= 12; i++)
            {
                col = 0;
                Local.Add(new InfoBarraPortico());
                Local[Local.Count - 1].C1 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C2 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C3 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C4 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C5 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C6 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C7 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C8 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].c9 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C10 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C11 = barra.MatrizLocal[i, ++col].ToString("n2");
                Local[Local.Count - 1].C12 = barra.MatrizLocal[i, ++col].ToString("n2");
            }
            gridLocal.DataSource = Local;

            Global = new List<InfoBarraPortico>();
            col = 0;
            for (int i = 1; i <= 12; i++)
            {
                col = 0;
                Global.Add(new InfoBarraPortico());
                Global[Global.Count - 1].C1 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C2 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C3 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C4 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C5 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C6 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C7 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C8 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].c9 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C10 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C11 = barra.MatrizGlobal[i, ++col].ToString("n2");
                Global[Global.Count - 1].C12 = barra.MatrizGlobal[i, ++col].ToString("n2");
            }
            gridGlobal.DataSource = Global;

            Rotacao = new List<InfoBarraPortico>();
            col = 0;
            for (int i = 1; i <= 12; i++)
            {
                col = 0;
                Rotacao.Add(new InfoBarraPortico());
                Rotacao[Rotacao.Count - 1].C1 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C2 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C3 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C4 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C5 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C6 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C7 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C8 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].c9 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C10 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C11 = barra.MatrizRotacao[i, ++col].ToString("n2");
                Rotacao[Rotacao.Count - 1].C12 = barra.MatrizRotacao[i, ++col].ToString("n2");
            }
            gridRotacao.DataSource = Rotacao;

            MatRotacaoTransposta = new List<InfoBarraPortico>();
            col = 0;
            for (int i = 1; i <= 12; i++)
            {
                col = 0;
                MatRotacaoTransposta.Add(new InfoBarraPortico());
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C1 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C2 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C3 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C4 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C5 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C6 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C7 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C8 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].c9 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C10 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C11 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
                MatRotacaoTransposta[MatRotacaoTransposta.Count - 1].C12 = barra.MatRotacaoTransposta[i, ++col].ToString("n2");
            }
            gridRotTransposta.DataSource = MatRotacaoTransposta;

            M = new List<InfoBarraPortico>();
            col = 0;
            for (int i = 1; i <= 12; i++)
            {
                col = 0;
                M.Add(new InfoBarraPortico());
                M[M.Count - 1].C1 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C2 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C3 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C4 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C5 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C6 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C7 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C8 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].c9 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C10 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C11 = barra.M[i, ++col].ToString("n2");
                M[M.Count - 1].C12 = barra.M[i, ++col].ToString("n2");
            }
            gridM.DataSource = M;

            lbBarra.Text = "Barra " + barra.IDBarra + " - pIni - Nro " + barra.pIni.Numero + " - x:" + barra.pIni.x.ToString("n2") + " y:" + barra.pIni.y.ToString("n2") + " z:" + barra.pIni.z.ToString("n2") +
                                                      "   pFin - Nro " + barra.pFin.Numero + " - x:" + barra.pFin.x.ToString("n2") + " y:" + barra.pFin.y.ToString("n2") + " z:" + barra.pFin.z.ToString("n2") +
                                                      " - comprimento: " + barra.L.ToString("n3") + " m";
            lbMat.Text = "E " + barra.E1 + "  -  Iy " + barra.Iy1 + " - Iz " + barra.Iz1 + "  - G " + barra.G1 + "  - J " + barra.J1 + "   - L " + barra.L.ToString("n3");
            label6.Text = "área: " + barra.A1.ToString("n4");

            label8.Text = "gl's pIni:" + barra.pIni.GlGlobal[1] + ","+barra.pIni.GlGlobal[2] + "," + barra.pIni.GlGlobal[3] + "," + barra.pIni.GlGlobal[4] + "," + barra.pIni.GlGlobal[5] + "," + barra.pIni.GlGlobal[6];
            label9.Text = "gl's pFin:" + barra.pFin.GlGlobal[1] + "," + barra.pFin.GlGlobal[2] + "," + barra.pFin.GlGlobal[3] + "," + barra.pFin.GlGlobal[4] + "," + barra.pFin.GlGlobal[5] + "," + barra.pFin.GlGlobal[6];
        }
        public bool mglobal;
        private void FInfoBarraPortico_Shown(object sender, EventArgs e)
        {
            atualiza();
        }

        private void GridLayers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GridLayers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

    }
}

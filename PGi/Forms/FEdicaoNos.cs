using MathNet.Numerics;
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

namespace PG
{
    public partial class FEdicaoNos : Form
    {
        public FEdicaoNos()
        {
            InitializeComponent();
        }
        public List<TBarraGenerica> objetos;
        Gerenciador gerenciador;
        public FEdicaoNos(Gerenciador _gerenciador)
        {
            InitializeComponent();
            this.gerenciador = _gerenciador;
            ShowInTaskbar = false;
        }

        private void FCopiaPadrao_Load(object sender, EventArgs e)
        {

            objetos = new List<TBarraGenerica>();
            string ss = "";
            if (!multi)
            if (gerenciador.formDesenho.ObjetosSelecionados[0].Tipo == Const.ID_PONTO)
            {
                TPonto pp = (TPonto)gerenciador.formDesenho.ObjetosSelecionados[0].Clone();
                foreach (TBarraGenerica o in gerenciador.formDesenho.Estrutura.barras)
                {
                    if (Geom.Iguais(o.pIni.x, pp.x) && Geom.Iguais(o.pIni.y, pp.y) && Geom.Iguais(o.pIni.z, pp.z))
                    {
                        ss += o.IDBarra + "  ";
                    }
                    else
                    if (Geom.Iguais(o.pFin.x, pp.x) && Geom.Iguais(o.pFin.y, pp.y) && Geom.Iguais(o.pFin.z, pp.z))
                    { 
                        ss += o.IDBarra + "  ";
                    }
                }
              
                edElementos.Text = ss;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {           
            this.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Enter(object sender, EventArgs e)
        {

        }
        public string Comando;
        public TPonto p1, p2;
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void FCopiaPadrao_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.fEditaNos = null;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
    
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
          }

        private void btSalvar_Click(object sender, EventArgs e)
        {
        }

        private void intervalo_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void ocorrencias_KeyUp(object sender, KeyEventArgs e)
        { 
        }

        private void qtdRepeticoesRotacao_ValueChanged(object sender, EventArgs e)
        {

        }

        private void edAngulo_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void edAngulo_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            // call base handler...
            base.OnKeyPress(e);
        }

        private void edAngulo_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        public bool multi;
        private void button1_Click(object sender, EventArgs e)
        {
          //  if (gerenciador.formDesenho.ObjetosSelecionados[0].Tipo == Const.ID_PONTO || multi)
            {
                TPonto pp = new TPonto(0,0,0);
                if (!multi)
                    pp = (TPonto)gerenciador.formDesenho.ObjetosSelecionados.FindAll(o => o.Tipo == Const.ID_PONTO).First() .Clone();

                List<TPonto> pts = gerenciador.formDesenho.Estrutura.nos.FindAll(o => o.Selecionado).ToList();
                double nx = 0, ny = 0, nz = 0;
                bool altX = true, altY = true, altZ = true;

                if (edX.Text != string.Empty)
                    nx = double.Parse(edX.Text);
                else
                    altX = false; 
  
                if (edY.Text != string.Empty)
                    ny = double.Parse(edY.Text);
                else
                    altY = false;

                if (edZ.Text != string.Empty)
                   nz = double.Parse(edZ.Text);
                else
                   altZ = false;

                gerenciador.ChamaAguardar(gerenciador, "Atualizando. Aguarde...");
                int total = 0;

                if (multi)
                    total = pts.Count;
                else
                    total = 1;

                for (int cc = 0; cc < total; cc++)
                {
                    if (multi)
                        pp = pts[cc];

                    foreach (TApoio a in gerenciador.formDesenho.Estrutura.apoios)
                    {
                        if (Geom.Iguais(pp.x, a.pIni.x) &&
                           Geom.Iguais(pp.y, a.pIni.y) &&
                           Geom.Iguais(pp.z, a.pIni.z))
                        {
                            if (altX)
                                a.pIni.x = nx;
                            if (altY)
                                a.pIni.y = -ny;
                            if (altZ)
                                a.pIni.z = -nz;
                        }
                    }

                    foreach (TBarraGenerica o in gerenciador.formDesenho.Estrutura.barras)
                    {
                        if (Geom.Iguais(o.pIni.x, pp.x) && Geom.Iguais(o.pIni.y, pp.y) && Geom.Iguais(o.pIni.z, pp.z))
                        {
                            if (altX)
                              o.pIni.x = nx;
                            if (altY)
                                o.pIni.y = -ny;
                            if (altZ)
                                o.pIni.z = -nz;

                            o.Linha_Eixo.pIni.x = o.pIni.x;
                            o.Linha_Eixo.pIni.y = o.pIni.y;
                            o.Linha_Eixo.pIni.z = o.pIni.z;

                            o.Linha_Eixo.pFin.x = o.pFin.x;
                            o.Linha_Eixo.pFin.y = o.pFin.y;
                            o.Linha_Eixo.pFin.z = o.pFin.z;

                            o.pMedioBarra = new vec3((o.pIni.x + o.pFin.x) / 2, (o.pIni.y + o.pFin.y) / 2, (o.pIni.z + o.pFin.z) / 2);
                            o.comprimento = (float)o.pFin.DistanceTo(o.pIni);
                        }
                        else
                        if (Geom.Iguais(o.pFin.x, pp.x) && Geom.Iguais(o.pFin.y, pp.y) && Geom.Iguais(o.pFin.z, pp.z))
                        {
                            if (altX)
                                o.pFin.x = nx;
                            if (altY)
                                o.pFin.y = -ny;
                            if (altZ)
                                o.pFin.z = -nz;

                            o.Linha_Eixo.pIni.x = o.pIni.x;
                            o.Linha_Eixo.pIni.y = o.pIni.y;
                            o.Linha_Eixo.pIni.z = o.pIni.z;

                            o.Linha_Eixo.pFin.x = o.pFin.x;
                            o.Linha_Eixo.pFin.y = o.pFin.y;
                            o.Linha_Eixo.pFin.z = o.pFin.z;

                            o.pMedioBarra = new vec3((o.pIni.x + o.pFin.x) / 2, (o.pIni.y + o.pFin.y) / 2, (o.pIni.z + o.pFin.z) / 2);
                            o.comprimento = (float)o.pFin.DistanceTo(o.pIni);
                        }
                    }

                    for (int i = 0; i < gerenciador.formDesenho.Estrutura.cargaPontual.Count; i++)
                    {
                        if (Geom.Iguais(gerenciador.formDesenho.Estrutura.cargaPontual[i].ponto.x, pp.x) && Geom.Iguais(gerenciador.formDesenho.Estrutura.cargaPontual[i].ponto.y, pp.y) && Geom.Iguais(gerenciador.formDesenho.Estrutura.cargaPontual[i].ponto.z, pp.z))

                        {
                            if (altX)
                                gerenciador.formDesenho.Estrutura.cargaPontual[i].pIni.x = nx;
                            if (altY)
                                gerenciador.formDesenho.Estrutura.cargaPontual[i].pIni.y = -ny;
                            if (altZ)
                                gerenciador.formDesenho.Estrutura.cargaPontual[i].pIni.z = -nz;

                            if (altX)
                                gerenciador.formDesenho.Estrutura.cargaPontual[i].ponto.x = nx;
                            if (altY)
                                gerenciador.formDesenho.Estrutura.cargaPontual[i].ponto.y = -ny;
                            if (altZ)
                                gerenciador.formDesenho.Estrutura.cargaPontual[i].ponto.z = -nz;
                        }
                    }

                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void FCopiaPadrao_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }
    }
}

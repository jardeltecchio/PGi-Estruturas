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
    public partial class FEsforcosPortico : Form
    {
        FVisualizadorPortico FVisPortico;
        Gerenciador gerenciador;
        public FEsforcosPortico()
        {
            InitializeComponent();
        }

        public FEsforcosPortico(FVisualizadorPortico _pai)
        {
            this.FVisPortico = _pai;
            InitializeComponent();
        }

        public FEsforcosPortico(Gerenciador _pai)
        {
            this.gerenciador = _pai;
            InitializeComponent();
        }
        void DiagramasPortico(object sender)
        {
            if (FVisPortico == null)
                return;
            try
            {
                //      ribbonPanelPortico.Text = "Pórtico Espacial - ";
                FVisPortico.Text = "Pórtico";
                FVisPortico.Deslocamento = false;
                FVisPortico.CortanteY = false;
                FVisPortico.CortanteZ = false;
                FVisPortico.Torcor = false;
                FVisPortico.Axial = false;

                if ((sender as Button) == btPorticoDz & btPorticoDz.Tag == "0")
                {
                    FVisPortico.Deslocamento = true;
                    FVisPortico.FletorZ = false;
                    FVisPortico.FletorY = false;

                    btPorticoDz.Tag = "1";
                    btPorticoFX.Tag = "0";
                    btPorticoFY.Tag = "0";
                    btPorticoFZ.Tag = "0";
                    btPorticoMX.Tag = "0";
                    btPorticoMY.Tag = "0";
                    btPorticoMZ.Tag = "0";
                    FVisPortico.Text = "Pórtico - Deslocamentos";
                    //      FVisGrelha.PanelCores2.Visible = ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DeslocamentoGradiente;
                }
                else
                    if ((sender as Button) == btPorticoDz & btPorticoDz.Tag == "1")
                        btPorticoDz.Tag = "0";

                if ((sender as Button) == btPorticoMY && btPorticoMY.Tag == "0")
                {
                    FVisPortico.FletorY = true;

                    btPorticoMY.Tag = "1";

                    btPorticoFX.Tag = "0";
                    btPorticoFY.Tag = "0";
                    btPorticoFZ.Tag = "0";
                    btPorticoMX.Tag = "0";
                    btPorticoDz.Tag = "0";

                    if (FVisPortico.FletorZ)
                    {
                        FVisPortico.MaximoEsforco = Math.Max(FVisPortico.Portico.MaximoEsforco(5), FVisPortico.Portico.MaximoEsforco(6));
                        FVisPortico.Text = "Pórtico - Fletores Y, Z";
                    }
                    else
                    {
                        FVisPortico.Text = "Pórtico - Fletores Y";
                        FVisPortico.MaximoEsforco = FVisPortico.Portico.MaximoEsforco(6);
                    }
                    FVisPortico.MaximoEsforco = 20 / FVisPortico.MaximoEsforco;
                }
                else
                    if ((sender as Button) == btPorticoMY && btPorticoMY.Tag == "1")
                    {
                        FVisPortico.FletorY = false;

                        btPorticoMY.Tag = "0";

                        if (btPorticoMZ.Tag == "1")
                        {
                            FVisPortico.FletorZ = true;
                            FVisPortico.Text = "Pórtico - Fletores Z";
                        }
                    }

                if ((sender as Button) == btPorticoMZ && btPorticoMZ.Tag == "0")
                {
                    FVisPortico.FletorZ = true;

                    btPorticoMZ.Tag = "1";

                    btPorticoFX.Tag = "0";
                    btPorticoFY.Tag = "0";
                    btPorticoFZ.Tag = "0";
                    btPorticoDz.Tag = "0";
                    btPorticoMX.Tag = "0";

                    if (FVisPortico.FletorY)
                    {
                        FVisPortico.MaximoEsforco = Math.Max(FVisPortico.Portico.MaximoEsforco(5), FVisPortico.Portico.MaximoEsforco(6));
                        FVisPortico.Text = "Pórtico - Fletores Z, Y";
                    }
                    else
                    {
                        FVisPortico.MaximoEsforco = FVisPortico.Portico.MaximoEsforco(5);
                        FVisPortico.Text = "Pórtico - Fletores Z";
                    }
                    FVisPortico.MaximoEsforco = 20 / FVisPortico.MaximoEsforco;

                    FVisPortico.iEsforcoAtual = FVisualizadorPortico.iFletorZ;
                }
                else
                    if ((sender as Button) == btPorticoMZ && btPorticoMZ.Tag == "1")
                    {
                        FVisPortico.FletorZ = false;

                        btPorticoMZ.Tag = "0";

                        if (btPorticoMY.Tag == "1")
                        {
                            FVisPortico.Text = "Pórtico - Fletores Y";
                            FVisPortico.FletorY = true;
                        }
                    }

                if ((sender as Button) == btPorticoMX && btPorticoMX.Tag == "0")
                {
                    FVisPortico.Torcor = true;
                    FVisPortico.FletorZ = false;
                    FVisPortico.FletorY = false;

                    btPorticoMY.Tag = "0";
                    btPorticoMZ.Tag = "0";
                    btPorticoFX.Tag = "0";
                    btPorticoMX.Tag = "1";
                    btPorticoFY.Tag = "0";
                    btPorticoFZ.Tag = "0";
                    btPorticoDz.Tag = "0";
                    FVisPortico.Text = "Pórtico - Torçores";
                    FVisPortico.MaximoEsforco = FVisPortico.Portico.MaximoEsforco(4);
                    FVisPortico.MaximoEsforco = 20 / FVisPortico.MaximoEsforco;
                }
                else
                    btPorticoMX.Tag = "0";

                if ((sender as Button) == btPorticoFX && btPorticoFX.Tag == "0")
                {
                    FVisPortico.Axial = true;
                    FVisPortico.FletorZ = false;
                    FVisPortico.FletorY = false;

                    btPorticoDz.Tag = "0";
                    btPorticoMX.Tag = "0";
                    btPorticoMY.Tag = "0";
                    btPorticoMZ.Tag = "0";
                    btPorticoFX.Tag = "1";
                    btPorticoFY.Tag = "0";
                    btPorticoFZ.Tag = "0";
                    FVisPortico.MaximoEsforco = 20 / FVisPortico.Portico.MaximoEsforco(1);
                    FVisPortico.Text = "Pórtico - Axiais";
                }
                else
                    btPorticoFX.Tag = "0";

                if ((sender as Button) == btPorticoFY && btPorticoFY.Tag == "0")
                {
                    FVisPortico.CortanteY = true;
                    FVisPortico.FletorZ = false;
                    FVisPortico.FletorY = false;

                    btPorticoDz.Tag = "0";
                    btPorticoMX.Tag = "0";
                    btPorticoMY.Tag = "0";
                    btPorticoMZ.Tag = "0";
                    btPorticoFX.Tag = "0";
                    btPorticoFY.Tag = "1";
                    FVisPortico.Text = "Pórtico - Cortantes Y";
                }
                else
                    if ((sender as Button) == btPorticoFY && btPorticoFY.Tag == "1")
                    {
                        FVisPortico.CortanteY = false;

                        btPorticoFY.Tag = "0";

                        if (btPorticoFZ.Tag == "1")
                        {
                            FVisPortico.Text = "Pórtico - Cortantes Z";
                            FVisPortico.CortanteZ = true;
                        }
                    }

                if ((sender as Button) == btPorticoFZ && btPorticoFZ.Tag == "0")
                {
                    FVisPortico.CortanteZ = true;
                    FVisPortico.FletorZ = false;
                    FVisPortico.FletorY = false;

                    btPorticoDz.Tag = "0";
                    btPorticoMX.Tag = "0";
                    btPorticoMY.Tag = "0";
                    btPorticoMZ.Tag = "0";
                    btPorticoFX.Tag = "0";
                    btPorticoFZ.Tag = "1";
                    FVisPortico.Text = "Pórtico - Cortantes Z";
                }
                else
                    if ((sender as Button) == btPorticoFZ && btPorticoFZ.Tag == "1")
                    {
                        FVisPortico.CortanteZ = false;

                        btPorticoFZ.Tag = "0";

                        if (btPorticoFY.Tag == "1")
                        {
                            FVisPortico.Text = "Pórtico - Cortantes Z";
                            FVisPortico.CortanteY = true;
                        }
                    }

                //    if (FVisPortico.iEsforcoAtual != FVisualizadorGrelha.iDeslocamento)
                //      FVisPortico.PanelCores2.Visible = ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_Gradiente;


                if (FVisPortico != null)
                {
                 //   FVisPortico.AtualizaFormCores();
                //    FVisPortico.Render();
                 //   FVisPortico.Controle.SwapBuffers();
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            //      TOpenGl.OGL.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DiagramasPortico(sender);
        }

        private void FEsforcosPortico_Shown(object sender, EventArgs e)
        {
            this.Left = 10;
            this.Top = 200;
        }

        private void btPorticoDz_MouseEnter(object sender, EventArgs e)
        {
            lbDica.Text = "Deslocamento";

        }

        private void btPorticoDz_MouseLeave(object sender, EventArgs e)
        {
            lbDica.Text = "";
        }

        private void btPorticoFY_MouseEnter(object sender, EventArgs e)
        {
            lbDica.Text = "Cortante Y";

        }

        private void btPorticoFZ_MouseEnter(object sender, EventArgs e)
        {
            lbDica.Text = "Cortante Z";
   
        }

        private void btPorticoFX_MouseEnter(object sender, EventArgs e)
        {
            lbDica.Text = "Axial";

        }

        private void btPorticoMY_MouseEnter(object sender, EventArgs e)
        {
            lbDica.Text = "Fletor Y";

        }

        private void btPorticoMZ_MouseEnter(object sender, EventArgs e)
        {
            lbDica.Text = "Fletor Z";

        }

        private void btPorticoMX_MouseEnter(object sender, EventArgs e)
        {
            lbDica.Text = "Torçor";

        }

        private void trackEscalaDiagrama_Scroll(object sender, EventArgs e)
        {
            AtualizaEscala();
            label2.Text = "x"+trackEscalaDiagrama.Value.ToString();
        }

        public void AtualizaEscala()
        {
            FVisPortico.iEscalaDiagrama = trackEscalaDiagrama.Value;
            FVisPortico.Render();
          //  TOpenGl.OGL.Refresh();
        }
        public void AtualizaEscalaCarga()
        {
            FVisPortico.iEscalaCarga = trackEscalaCarga.Value/2;
            FVisPortico.Render();
        //    TOpenGl.OGL.Refresh();
        }
        private void chMostrarCargasLineares_CheckedChanged(object sender, EventArgs e)
        {
            FVisPortico.CargaLinear = chMostrarCargasLineares.Checked ;
            FVisPortico.Render();
     //       TOpenGl.OGL.Refresh();
        }

        private void chMostrarCargasPontuais_CheckedChanged(object sender, EventArgs e)
        {
            FVisPortico.CargaPontual = chMostrarCargasPontuais.Checked;
            FVisPortico.Render();
        //    TOpenGl.OGL.Refresh();
        }

        private void trackEscalaCarga_Scroll(object sender, EventArgs e)
        {
            AtualizaEscalaCarga();
        }

        private void FEsforcosPortico_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        
        }
    }
}
